using System;
using System.Collections.Generic;
using System.Linq;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.Route.Context;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingRoute.Engine.Condition;
using OpenConnector.ShardingRoute.Engine.RouteType.Standard;

namespace OpenConnector.ShardingRoute.Engine.RouteType.Complex
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Wednesday, 28 April 2021 22:01:04
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingComplexRoutingEngine : IShardingRouteEngine
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
            foreach (var logicTable in logicTables)
            {
                var tableRule = shardingRule.FindTableRule(logicTable);
                if (tableRule != null)
                {
                    if (!bindingTableNames.Contains(logicTable))
                    {
                        result.Add(new ShardingStandardRoutingEngine(tableRule.LogicTable, sqlStatementContext, shardingConditions, properties).Route(shardingRule));
                    }
                    shardingRule.FindBindingTableRule(logicTable).IfPresent(bindingTableRule => bindingTableNames.AddAll(
                        bindingTableRule.GetTableRules().Select(o => o.LogicTable).ToList()));
                }
            }
            if (result.IsEmpty())
            {
                throw new ShardingException($"Cannot find table rule and default data source with logic tables: '{logicTables}'");
            }
            if (1 == result.Count)
            {
                return result.First();
            }
            return new ShardingCartesianRoutingEngine(result).Route(shardingRule);
        }
    }
}