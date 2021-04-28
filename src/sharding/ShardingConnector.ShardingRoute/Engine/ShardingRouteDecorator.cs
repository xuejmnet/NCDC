using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Route;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

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
        public RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, ShardingRule rule,
            ConfigurationProperties properties)
        {
            var sqlStatementContext = routeContext.GetSqlCommandContext();
            List<object> parameters = routeContext.GetParameters();
            ShardingStatementValidatorFactory.newInstance(
                    sqlStatementContext.getSqlStatement()).ifPresent(validator->validator.validate(shardingRule, sqlStatementContext.getSqlStatement(), parameters));
            ShardingConditions shardingConditions = getShardingConditions(parameters, sqlStatementContext, metaData.getSchema(), shardingRule);
            boolean needMergeShardingValues = isNeedMergeShardingValues(sqlStatementContext, shardingRule);
            if (sqlStatementContext.getSqlStatement() instanceof DMLStatement && needMergeShardingValues) {
                checkSubqueryShardingValues(sqlStatementContext, shardingRule, shardingConditions);
                mergeShardingConditions(shardingConditions);
            }
            ShardingRouteEngine shardingRouteEngine = ShardingRouteEngineFactory.newInstance(shardingRule, metaData, sqlStatementContext, shardingConditions, properties);
            RouteResult routeResult = shardingRouteEngine.route(shardingRule);
            if (needMergeShardingValues)
            {
                Preconditions.checkState(1 == routeResult.getRouteUnits().size(), "Must have one sharding with subquery.");
            }
            return new RouteContext(sqlStatementContext, parameters, routeResult);
        }

        private ShardingConditions getShardingConditions(final List<Object> parameters,
                                                         final SQLStatementContext sqlStatementContext, final SchemaMetaData schemaMetaData, final ShardingRule shardingRule)
        {
            if (sqlStatementContext.getSqlStatement() instanceof DMLStatement) {
                if (sqlStatementContext instanceof InsertStatementContext) {
                    return new ShardingConditions(new InsertClauseShardingConditionEngine(shardingRule).createShardingConditions((InsertStatementContext)sqlStatementContext, parameters));
                }
                return new ShardingConditions(new WhereClauseShardingConditionEngine(shardingRule, schemaMetaData).createShardingConditions(sqlStatementContext, parameters));
            }
            return new ShardingConditions(Collections.emptyList());
        }

        private boolean isNeedMergeShardingValues(final SQLStatementContext sqlStatementContext, final ShardingRule shardingRule)
        {
            return sqlStatementContext instanceof SelectStatementContext && ((SelectStatementContext)sqlStatementContext).isContainsSubquery()
                    && !shardingRule.getShardingLogicTableNames(sqlStatementContext.getTablesContext().getTableNames()).isEmpty();
        }

        private void checkSubqueryShardingValues(final SQLStatementContext sqlStatementContext, final ShardingRule shardingRule, final ShardingConditions shardingConditions)
        {
            for (String each : sqlStatementContext.getTablesContext().getTableNames())
            {
                Optional<TableRule> tableRule = shardingRule.findTableRule(each);
                if (tableRule.isPresent() && isRoutingByHint(shardingRule, tableRule.get())
                        && !HintManager.getDatabaseShardingValues(each).isEmpty() && !HintManager.getTableShardingValues(each).isEmpty())
                {
                    return;
                }
            }
            Preconditions.checkState(!shardingConditions.getConditions().isEmpty(), "Must have sharding column with subquery.");
            if (shardingConditions.getConditions().size() > 1)
            {
                Preconditions.checkState(isSameShardingCondition(shardingRule, shardingConditions), "Sharding value must same with subquery.");
            }
        }

        private boolean isRoutingByHint(final ShardingRule shardingRule, final TableRule tableRule)
        {
            return shardingRule.getDatabaseShardingStrategy(tableRule) instanceof HintShardingStrategy && shardingRule.getTableShardingStrategy(tableRule) instanceof HintShardingStrategy;
        }

        private boolean isSameShardingCondition(final ShardingRule shardingRule, final ShardingConditions shardingConditions)
        {
            ShardingCondition example = shardingConditions.getConditions().remove(shardingConditions.getConditions().size() - 1);
            for (ShardingCondition each : shardingConditions.getConditions())
            {
                if (!isSameShardingCondition(shardingRule, example, each))
                {
                    return false;
                }
            }
            return true;
        }

        private boolean isSameShardingCondition(final ShardingRule shardingRule, final ShardingCondition shardingCondition1, final ShardingCondition shardingCondition2)
        {
            if (shardingCondition1.getRouteValues().size() != shardingCondition2.getRouteValues().size())
            {
                return false;
            }
            for (int i = 0; i < shardingCondition1.getRouteValues().size(); i++)
            {
                RouteValue shardingValue1 = shardingCondition1.getRouteValues().get(i);
                RouteValue shardingValue2 = shardingCondition2.getRouteValues().get(i);
                if (!isSameRouteValue(shardingRule, (ListRouteValue)shardingValue1, (ListRouteValue)shardingValue2))
                {
                    return false;
                }
            }
            return true;
        }

        private boolean isSameRouteValue(final ShardingRule shardingRule, final ListRouteValue routeValue1, final ListRouteValue routeValue2)
        {
            return isSameLogicTable(shardingRule, routeValue1, routeValue2) && routeValue1.getColumnName().equals(routeValue2.getColumnName()) && routeValue1.getValues().equals(routeValue2.getValues());
        }

        private boolean isSameLogicTable(final ShardingRule shardingRule, final ListRouteValue shardingValue1, final ListRouteValue shardingValue2)
        {
            return shardingValue1.getTableName().equals(shardingValue2.getTableName()) || isBindingTable(shardingRule, shardingValue1, shardingValue2);
        }

        private boolean isBindingTable(final ShardingRule shardingRule, final ListRouteValue shardingValue1, final ListRouteValue shardingValue2)
        {
            Optional<BindingTableRule> bindingRule = shardingRule.findBindingTableRule(shardingValue1.getTableName());
            return bindingRule.isPresent() && bindingRule.get().hasLogicTable(shardingValue2.getTableName());
        }

        private void mergeShardingConditions(final ShardingConditions shardingConditions)
        {
            if (shardingConditions.getConditions().size() > 1)
            {
                ShardingCondition shardingCondition = shardingConditions.getConditions().remove(shardingConditions.getConditions().size() - 1);
                shardingConditions.getConditions().clear();
                shardingConditions.getConditions().add(shardingCondition);
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
