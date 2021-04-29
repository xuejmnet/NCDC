using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen;
using ShardingConnector.ParserBinder.Segment.Insert.Values;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.SPI;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:44:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertClauseShardingConditionEngine
    {
        private readonly ShardingRule _shardingRule;

        public InsertClauseShardingConditionEngine(ShardingRule shardingRule)
        {
            this._shardingRule = shardingRule;
        }

        /**
         * Create sharding conditions.
         * 
         * @param insertCommandContext insert statement context
         * @param parameters SQL parameters
         * @return sharding conditions
         */
        public List<ShardingCondition> CreateShardingConditions(InsertCommandContext insertCommandContext,
            List<object> parameters)
        {
            ICollection<ShardingCondition> result = new LinkedList<ShardingCondition>();
            string tableName = insertCommandContext.GetSqlCommand().Table.GetTableName().GetIdentifier().GetValue();
            ICollection<string> columnNames = GetColumnNames(insertCommandContext);
            foreach (var insertValueContext in insertCommandContext.GetInsertValueContexts())
            {
                result.Add(CreateShardingCondition(tableName, columnNames.GetEnumerator(), insertValueContext,
                    parameters));
            }

            var generatedKey = insertCommandContext.GetGeneratedKeyContext();
            if (generatedKey != null && generatedKey.IsGenerated())
            {
                generatedKey.GetGeneratedValues().AddAll(GetGeneratedKeys(tableName,
                    insertCommandContext.GetSqlCommand().GetValueListCount()));
                if (_shardingRule.IsShardingColumn(generatedKey.GetColumnName(), tableName))
                {
                    AppendGeneratedKeyCondition(generatedKey, tableName, result);
                }
            }

            return result.ToList();
        }

        private ICollection<string> GetColumnNames(InsertCommandContext insertCommandContext)
        {
            var generatedKey = insertCommandContext.GetGeneratedKeyContext();
            if (generatedKey != null && generatedKey.IsGenerated())
            {
                ICollection<string> result = new LinkedList<string>(insertCommandContext.GetColumnNames());
                result.Remove(generatedKey.GetColumnName());
                return result;
            }

            return insertCommandContext.GetColumnNames();
        }

        private ShardingCondition CreateShardingCondition(string tableName, IEnumerator<string> columnNames,
            InsertValueContext insertValueContext, List<object> parameters)
        {
            ShardingCondition result = new ShardingCondition();
            SPITimeService timeService = SPITimeService.GetInstance();


            foreach (var valueExpression in insertValueContext.GetValueExpressions())
            {
                string columnName = columnNames.Next();
                if (_shardingRule.IsShardingColumn(columnName, tableName))
                {
                    if (valueExpression is ISimpleExpressionSegment simpleExpressionSegment)
                    {
                        result.RouteValues.Add(new ListRouteValue(columnName, tableName,
                            new List<IComparable>() { GetRouteValue(simpleExpressionSegment, parameters) }));
                    }
                    else if (ExpressionConditionUtils.IsNowExpression(valueExpression))
                    {
                        result.RouteValues.Add(new ListRouteValue(columnName, tableName,
                            new List<IComparable>() { timeService.GetTime() }));
                    }
                    else if (ExpressionConditionUtils.IsNullExpression(valueExpression))
                    {
                        throw new ShardingException("Insert clause sharding column can't be null.");
                    }
                }
            }

            return result;
        }

        private IComparable GetRouteValue(ISimpleExpressionSegment expressionSegment, List<object> parameters)
        {
            object result;
            if (expressionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                result = parameters[parameterMarkerExpressionSegment.GetParameterMarkerIndex()];
            }
            else
            {
                result = ((LiteralExpressionSegment)expressionSegment).GetLiterals();
            }

            ShardingAssert.Else(result is IComparable, "Sharding value must implements IComparable.");
            return (IComparable)result;
        }

        private ICollection<IComparable> GetGeneratedKeys(string tableName, int valueListCount)
        {
            return Enumerable.Range(0, valueListCount).Select(o => _shardingRule.GenerateKey(tableName)).ToList();
        }

        private void AppendGeneratedKeyCondition(GeneratedKeyContext generatedKey, string tableName,
            ICollection<ShardingCondition> shardingConditions)
        {
            var enumerator = generatedKey.GetGeneratedValues().GetEnumerator();
            foreach (var shardingCondition in shardingConditions)
            {
                shardingCondition.RouteValues.Add(new ListRouteValue(generatedKey.GetColumnName(),
                    tableName, new List<IComparable>() { enumerator.Next() }));

            }

        }
    }
}
