using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.DefaultDB
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 22:12:54
* @Email: 326308290@qq.com
*/
    public sealed class ShardingDefaultDatabaseRoutingEngine:IShardingRouteEngine
    {
        private readonly ICollection<String> _logicTables;

        public ShardingDefaultDatabaseRoutingEngine(ICollection<string> logicTables)
        {
            this._logicTables = logicTables;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            List<RouteMapper> routingTables = new List<RouteMapper>(_logicTables.Count);
            foreach (var logicTable in _logicTables)
            {
                routingTables.Add(new RouteMapper(logicTable, logicTable));
            }
            String dataSourceName = shardingRule.ShardingDataSourceNames.GetDefaultDataSourceName();
            result.GetRouteUnits().Add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), routingTables));
            return result;
        }
    }
}