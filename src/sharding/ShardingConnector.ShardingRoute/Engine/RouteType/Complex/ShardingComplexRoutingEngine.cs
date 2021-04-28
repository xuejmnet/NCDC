using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingRoute.Engine.Condition;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Complex
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 22:01:04
* @Email: 326308290@qq.com
*/
    public sealed class ShardingComplexRoutingEngine:IShardingRouteEngine
    {
        private readonly ICollection<String> logicTables;
    
        private readonly ISqlCommandContext<ISqlCommand> sqlStatementContext;
    
        private readonly ShardingConditions shardingConditions;

        private readonly ConfigurationProperties properties;

        public ShardingComplexRoutingEngine(ICollection<string> logicTables, ISqlCommandContext<ISqlCommand> sqlStatementContext, ShardingConditions shardingConditions, ConfigurationProperties properties)
        {
            this.logicTables = logicTables;
            this.sqlStatementContext = sqlStatementContext;
            this.shardingConditions = shardingConditions;
            this.properties = properties;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            ICollection<RouteResult> result = new List<RouteResult>(logicTables.Count);
            ICollection<String> bindingTableNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            // for (String each : logicTables) {
            //     Optional<TableRule> tableRule = shardingRule.findTableRule(each);
            //     if (tableRule.isPresent()) {
            //         if (!bindingTableNames.contains(each)) {
            //             result.add(new ShardingStandardRoutingEngine(tableRule.get().getLogicTable(), sqlStatementContext, shardingConditions, properties).route(shardingRule));
            //         }
            //         shardingRule.findBindingTableRule(each).ifPresent(bindingTableRule -> bindingTableNames.addAll(
            //             bindingTableRule.getTableRules().stream().map(TableRule::getLogicTable).collect(Collectors.toList())));
            //     }
            // }
            foreach (var logicTable in logicTables)
            {
                var tableRule = shardingRule.FindTableRule(logicTable);
                if (tableRule!=null) {
                    if (!bindingTableNames.Contains(logicTable)) {
                        result.Add(new ShardingStandardRoutingEngine(tableRule.get().getLogicTable(), sqlStatementContext, shardingConditions, properties).route(shardingRule));
                    }
                    shardingRule.findBindingTableRule(each).ifPresent(bindingTableRule -> bindingTableNames.addAll(
                        bindingTableRule.getTableRules().stream().map(TableRule::getLogicTable).collect(Collectors.toList())));
                }
            }
            if (result.isEmpty()) {
                throw new ShardingSphereException("Cannot find table rule and default data source with logic tables: '%s'", logicTables);
            }
            if (1 == result.size()) {
                return result.iterator().next();
            }
            return new ShardingCartesianRoutingEngine(result).route(shardingRule);
        }
    }
}