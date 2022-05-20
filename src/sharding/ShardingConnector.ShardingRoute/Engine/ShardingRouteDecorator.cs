using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.Route;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingApi.Api.Hint;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Hint;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.Engine.Condition;
using ShardingConnector.ShardingRoute.Engine.Condition.Engine;
using ShardingConnector.ShardingRoute.Engine.RouteType;
using ShardingConnector.ShardingRoute.Engine.Validator;

namespace ShardingConnector.ShardingRoute.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 10:09:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingRouteDecorator: IRouteDecorator<ShardingRule>
    {
        public RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, ShardingRule shardingRule,
            ConfigurationProperties properties)
        {
            var sqlStatementContext = routeContext.GetSqlCommandContext();
            var parameters = routeContext.GetParameters();
            ShardingCommandValidatorFactory.NewInstance(
                sqlStatementContext.GetSqlCommand())
                .IfPresent(validator=> validator.Validate(shardingRule, sqlStatementContext.GetSqlCommand(), parameters));
            ShardingConditions shardingConditions = GetShardingConditions(parameters, sqlStatementContext, metaData.Schema, shardingRule);
            var needMergeShardingValues = IsNeedMergeShardingValues(sqlStatementContext, shardingRule);
            if (needMergeShardingValues && sqlStatementContext.GetSqlCommand() is DMLCommand)
            {
                CheckSubQueryShardingValues(sqlStatementContext, shardingRule, shardingConditions);
                MergeShardingConditions(shardingConditions);
            }
            var shardingRouteEngine = ShardingRouteEngineFactory.NewInstance(shardingRule, metaData, sqlStatementContext, shardingConditions, properties);
            RouteResult routeResult = shardingRouteEngine.Route(shardingRule);
            if (needMergeShardingValues)
            {
                ShardingAssert.Else(1 == routeResult.GetRouteUnits().Count, "Must have one sharding with sub query.");
            }
            return new RouteContext(sqlStatementContext, parameters, routeResult);
        }

        private ShardingConditions GetShardingConditions(IDictionary<string, DbParameter> parameters,
                                                         ISqlCommandContext<ISqlCommand> sqlStatementContext, SchemaMetaData schemaMetaData, ShardingRule shardingRule)
        {
            if (sqlStatementContext.GetSqlCommand() is DMLCommand) {
                if (sqlStatementContext is InsertCommandContext insertCommandContext) {
                    return new ShardingConditions(new InsertClauseShardingConditionEngine(shardingRule).CreateShardingConditions(insertCommandContext, parameters));
                }
                return new ShardingConditions(new WhereClauseShardingConditionEngine(shardingRule, schemaMetaData).CreateShardingConditions(sqlStatementContext, parameters));
            }
            return new ShardingConditions(new List<ShardingCondition>(0));
        }

        private bool IsNeedMergeShardingValues(ISqlCommandContext<ISqlCommand> sqlStatementContext, ShardingRule shardingRule)
        {
            return sqlStatementContext is SelectCommandContext selectCommandContext && selectCommandContext.IsContainsSubQuery()
                    && shardingRule.GetShardingLogicTableNames(sqlStatementContext.GetTablesContext().GetTableNames()).Any();
        }

        private void CheckSubQueryShardingValues(ISqlCommandContext<ISqlCommand> sqlStatementContext, ShardingRule shardingRule, ShardingConditions shardingConditions)
        {
            foreach (var tableName in sqlStatementContext.GetTablesContext().GetTableNames())
            {
                var tableRule = shardingRule.FindTableRule(tableName);
                if (tableRule!=null && IsRoutingByHint(shardingRule, tableRule)
                                          && HintManager.GetDatabaseShardingValues(tableName).Any() && HintManager.GetTableShardingValues(tableName).Any())
                {
                    return;
                }
            }
            ShardingAssert.If(shardingConditions.Conditions.IsEmpty(), "Must have sharding column with subquery.");
            if (shardingConditions.Conditions.Count > 1)
            {
                ShardingAssert.Else(IsSameShardingCondition(shardingRule, shardingConditions), "Sharding value must same with subquery.");
            }
        }

        private bool IsRoutingByHint(ShardingRule shardingRule, TableRule tableRule)
        {
            return shardingRule.GetDatabaseShardingStrategy(tableRule) is HintShardingStrategy && shardingRule.GetTableShardingStrategy(tableRule) is HintShardingStrategy;
        }

        private bool IsSameShardingCondition(ShardingRule shardingRule, ShardingConditions shardingConditions)
        {
            ShardingCondition example = shardingConditions.Conditions.Last();
            shardingConditions.Conditions.RemoveAt(shardingConditions.Conditions.Count-1);
            foreach (var condition in shardingConditions.Conditions)
            {
                if (!IsSameShardingCondition(shardingRule, example, condition))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSameShardingCondition(ShardingRule shardingRule, ShardingCondition shardingCondition1, ShardingCondition shardingCondition2)
        {
            if (shardingCondition1.RouteValues.Count != shardingCondition2.RouteValues.Count)
            {
                return false;
            }
            for (int i = 0; i < shardingCondition1.RouteValues.Count; i++)
            {
                var shardingValue1 = shardingCondition1.RouteValues.ElementAt(i);
                var shardingValue2 = shardingCondition2.RouteValues.ElementAt(i);
                if (!IsSameRouteValue(shardingRule, (ListRouteValue)shardingValue1, (ListRouteValue)shardingValue2))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSameRouteValue(ShardingRule shardingRule, ListRouteValue routeValue1, ListRouteValue routeValue2)
        {
            return IsSameLogicTable(shardingRule, routeValue1, routeValue2) && routeValue1.GetColumnName().Equals(routeValue2.GetColumnName()) && routeValue1.GetValues().SequenceEqual(routeValue2.GetValues());
        }

        private bool IsSameLogicTable(ShardingRule shardingRule, ListRouteValue shardingValue1, ListRouteValue shardingValue2)
        {
            return shardingValue1.GetTableName().Equals(shardingValue2.GetTableName()) || IsBindingTable(shardingRule, shardingValue1, shardingValue2);
        }

        private bool IsBindingTable(ShardingRule shardingRule, ListRouteValue shardingValue1, ListRouteValue shardingValue2)
        {
            var bindingRule = shardingRule.FindBindingTableRule(shardingValue1.GetTableName());
            return bindingRule!=null && bindingRule.HasLogicTable(shardingValue2.GetTableName());
        }

        private void MergeShardingConditions(ShardingConditions shardingConditions)
        {
            if (shardingConditions.Conditions.Count > 1)
            {
                ShardingCondition shardingCondition =
                    shardingConditions.Conditions[shardingConditions.Conditions.Count - 1];
                shardingConditions.Conditions.RemoveAt(shardingConditions.Conditions.Count - 1);
                shardingConditions.Conditions.Clear();
                shardingConditions.Conditions.Add(shardingCondition);
            }
        }

        public RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, IBaseRule rule,
            ConfigurationProperties properties)
        {
            return Decorate(routeContext, metaData,(ShardingRule) rule, properties);
        }

        public int GetOrder()
        {
            return 0;
        }

        public Type GetGenericType()
        {
            return typeof(ShardingRule);
        }
    }
}
