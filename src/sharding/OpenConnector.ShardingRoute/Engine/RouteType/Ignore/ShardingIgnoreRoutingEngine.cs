using System;
using OpenConnector.Route.Context;
using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.ShardingRoute.Engine.RouteType.Ignore
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