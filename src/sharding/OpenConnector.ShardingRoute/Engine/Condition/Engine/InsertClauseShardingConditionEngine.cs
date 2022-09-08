using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using OpenConnector.Base;
using OpenConnector.CommandParser.Extensions;
using OpenConnector.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.CommandParserBinder.Segment.Insert.Keygen;
using OpenConnector.CommandParserBinder.Segment.Insert.Values;
using OpenConnector.ShardingAdoNet;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;
using OpenConnector.ShardingRoute.SPI;

namespace OpenConnector.ShardingRoute.Engine.Condition.Engine
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
        private readonly ITableMetadataManager _tableMetadataManager;

        public InsertClauseShardingConditionEngine(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        /**
         * Create sharding conditions.
         * 
         * @param insertCommandContext insert statement context
         * @param parameters SQL parameters
         * @return sharding conditions
         */
        public List<ShardingCondition> CreateShardingConditions(InsertCommandContext insertCommandContext,
            ParameterContext parameterContext)
        {
            ICollection<ShardingCondition> result = new LinkedList<ShardingCondition>();
            string tableName = insertCommandContext.GetSqlCommand().Table.GetTableName().GetIdentifier().GetValue();
            ICollection<string> columnNames = GetColumnNames(insertCommandContext);
            foreach (var insertValueContext in insertCommandContext.GetInsertValueContexts())
            {
                result.Add(CreateShardingCondition(tableName, columnNames.GetEnumerator(), insertValueContext,
                    parameterContext));
            }

            var generatedKey = insertCommandContext.GetGeneratedKeyContext();
            if (generatedKey != null && generatedKey.IsGenerated())
            {
                generatedKey.GetGeneratedValues().AddAll(GetGeneratedKeys(tableName,
                    insertCommandContext.GetSqlCommand().GetValueListCount()));
                if (_tableMetadataManager.IsShardingColumn(tableName,generatedKey.GetColumnName()))
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
            InsertValueContext insertValueContext, ParameterContext parameterContext)
        {
            ShardingCondition result = new ShardingCondition();
            SPITimeService timeService = SPITimeService.GetInstance();


            foreach (var valueExpression in insertValueContext.GetValueExpressions())
            {
                string columnName = columnNames.Next();
                if (_tableMetadataManager.IsShardingColumn(tableName,columnName))
                {
                    if (valueExpression is ISimpleExpressionSegment simpleExpressionSegment)
                    {
                        result.RouteValues.Add(new ListRouteValue(columnName, tableName,
                            new List<IComparable>() { GetRouteValue(simpleExpressionSegment, parameterContext) }));
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

        private IComparable GetRouteValue(ISimpleExpressionSegment expressionSegment, ParameterContext parameterContext)
        {
            object result;
            if (expressionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                result = parameterContext.GetParameterValue(parameterMarkerExpressionSegment.GetParameterName());
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
            return Enumerable.Range(0, valueListCount).Select(o => _tableMetadataManager.GetGenerateKey(tableName)).ToList();
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
