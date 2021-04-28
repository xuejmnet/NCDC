using System;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Ignore
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 22:14:54
* @Email: 326308290@qq.com
*/
    public sealed class ShardingIgnoreRoutingEngine:IShardingRouteEngine
    {
        public RouteResult Route(ShardingRule shardingRule)
        {
            return new RouteResult();
        }
    }
}