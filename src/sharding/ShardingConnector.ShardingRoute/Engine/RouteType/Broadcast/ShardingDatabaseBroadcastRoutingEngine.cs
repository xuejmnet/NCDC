using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Broadcast
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:38:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDatabaseBroadcastRoutingEngine:IShardingRouteEngine
    {
        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            foreach (var dataSourceName in shardingRule.ShardingDataSourceNames.DataSourceNames)
            {

                result.GetRouteUnits().Add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), new List<RouteMapper>(0)));
            }
            return result;
        }
    }
}
