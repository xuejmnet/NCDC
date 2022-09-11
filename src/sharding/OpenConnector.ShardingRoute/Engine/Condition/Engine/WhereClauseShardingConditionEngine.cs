using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Segment.DML.Predicate;
using OpenConnector.DataStructure.RangeStructure;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.MetaData.Schema;
using OpenConnector.ShardingAdoNet;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;
using OpenConnector.ShardingRoute.Engine.Condition.Generator;

namespace OpenConnector.ShardingRoute.Engine.Condition.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 14:51:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class WhereClauseShardingConditionEngine
    {
        private readonly ShardingRule shardingRule;

        private readonly SchemaMetaData schemaMetaData;

        public WhereClauseShardingConditionEngine(ShardingRule shardingRule, SchemaMetaData schemaMetaData)
        {
            this.shardingRule = shardingRule;
            this.schemaMetaData = schemaMetaData;
        }

        /**
         * Create sharding conditions.
         * 
         * @param sqlStatementContext SQL statement context
         * @param parameters SQL parameters
         * @return sharding conditions
         */
        public List<ShardingCondition> CreateShardingConditions(ISqlCommandContext<ISqlCommand> sqlCommandContext,
            ParameterContext parameterContext)
        {
            if (sqlCommandContext is IWhereAvailable whereAvailable)
            {
                var whereSegment = whereAvailable.GetWhere();
                if (whereSegment != null)
                {
                    var shardingConditions = CreateShardingConditions(sqlCommandContext,
                        whereSegment.GetAndPredicates(), parameterContext);
                    return new List<ShardingCondition>(shardingConditions);
                }
            }

            return new List<ShardingCondition>(0);

            // FIXME process subquery
            //        ICollection<SubqueryPredicateSegment> subqueryPredicateSegments = sqlStatementContext.findSQLSegments(SubqueryPredicateSegment.class);
            //        for (SubqueryPredicateSegment each : subqueryPredicateSegments) {
            //            ICollection<ShardingCondition> subqueryShardingConditions = createShardingConditions((WhereSegmentAvailable) sqlStatementContext, each.getAndPredicates(), parameters);
            //            if (!result.containsAll(subqueryShardingConditions)) {
            //                result.addAll(subqueryShardingConditions);
            //            }
            //        }
        }

        private ICollection<ShardingCondition> CreateShardingConditions(
            ISqlCommandContext<ISqlCommand> sqlCommandContext, ICollection<AndPredicateSegment> andPredicates,
            ParameterContext parameterContext)
        {
            ICollection<ShardingCondition> result = new LinkedList<ShardingCondition>();
            foreach (var andPredicate in andPredicates)
            {
                var routeValueMap = CreateRouteValueMap(sqlCommandContext, andPredicate, parameterContext);
                if (routeValueMap.IsEmpty())
                {
                    return new List<ShardingCondition>(0);
                }

                result.Add(CreateShardingCondition(routeValueMap));
            }

            return result;
        }

        private IDictionary<Column, ICollection<IRouteValue>> CreateRouteValueMap(
            ISqlCommandContext<ISqlCommand> sqlCommandContext, AndPredicateSegment andPredicate,
            ParameterContext parameterContext)
        {
            IDictionary<Column, ICollection<IRouteValue>> result = new Dictionary<Column, ICollection<IRouteValue>>();
            foreach (var predicate in andPredicate.GetPredicates())
            {
                var tableName = sqlCommandContext.GetTablesContext()
                    .FindTableName(predicate.GetColumn(), null);
                if (tableName == null ||
                    !shardingRule.IsShardingColumn(predicate.GetColumn().GetIdentifier().GetValue(), tableName))
                {
                    continue;
                }

                Column column = new Column(predicate.GetColumn().GetIdentifier().GetValue(), tableName);
                var routeValue =
                    ConditionValueGeneratorFactory.Generate(predicate.GetPredicateRightValue(), column,
                        parameterContext);
                if (routeValue == null)
                {
                    continue;
                }

                if (!result.ContainsKey(column))
                {
                    result.Add(column, new LinkedList<IRouteValue>());
                }

                result[column].Add(routeValue);
            }

            return result;
        }

        private ShardingCondition CreateShardingCondition(IDictionary<Column, ICollection<IRouteValue>> routeValueMap)
        {
            ShardingCondition result = new ShardingCondition();
            foreach (var routeValueKv in routeValueMap)
            {
                try
                {
                    var routeValue = MergeRouteValues(routeValueKv.Key, routeValueKv.Value);
                    if (routeValue is AlwaysFalseRouteValue)
                    {
                        return new AlwaysFalseShardingCondition();
                    }

                    result.RouteValues.Add(routeValue);
                }
                catch (InvalidCastException ex)
                {
                    throw new ShardingException("Found different types for sharding value `{entry.Key}`.");
                }
            }

            return result;
        }

        private IRouteValue MergeRouteValues(Column column, ICollection<IRouteValue> routeValues)
        {
            ICollection<IComparable> listValue = null;
            Range<IComparable> rangeValue = null;
            foreach (var routeValue in routeValues)
            {
                if (routeValue is ListRouteValue listRouteValue)
                {
                    listValue = MergeListRouteValues(listRouteValue.GetValues(), listValue);
                    if (listValue.IsEmpty())
                    {
                        return new AlwaysFalseRouteValue();
                    }
                }
                else if (routeValue is RangeRouteValue rangeRouteValue)
                {
                    try
                    {
                        rangeValue = MergeRangeRouteValues(rangeRouteValue.GetValueRange(), rangeValue);
                    }
                    catch (InvalidOperationException ex)
                    {
                        return new AlwaysFalseRouteValue();
                    }
                }
            }

            if (null == listValue)
            {
                return new RangeRouteValue(column.Name, column.TableName, rangeValue);
            }

            if (null == rangeValue)
            {
                return new ListRouteValue(column.Name, column.TableName, listValue);
            }

            listValue = MergeListAndRangeRouteValues(listValue, rangeValue);
            if (listValue.IsEmpty())
                return new AlwaysFalseRouteValue();
            return new ListRouteValue(column.Name, column.TableName, listValue);
        }

        private ICollection<IComparable> MergeListRouteValues(ICollection<IComparable> value1,
            ICollection<IComparable> value2)
        {
            if (null == value2)
            {
                return value1;
            }

            value1 = value1.Intersect(value2).ToList();
            return value1;
        }

        private Range<IComparable> MergeRangeRouteValues(Range<IComparable> value1, Range<IComparable> value2)
        {
            return null == value2 ? value1 : value1.Intersection(value2);
        }

        private ICollection<IComparable> MergeListAndRangeRouteValues(ICollection<IComparable> listValue,
            Range<IComparable> rangeValue)
        {
            ICollection<IComparable> result = new LinkedList<IComparable>();
            foreach (var value in listValue)
            {
                if (rangeValue.Contains(value))
                {
                    result.Add(value);
                }
            }

            return result;
        }
    }
}