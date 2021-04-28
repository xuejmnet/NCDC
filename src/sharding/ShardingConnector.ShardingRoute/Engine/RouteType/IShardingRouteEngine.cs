using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:34:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IShardingRouteEngine
    {
        RouteResult Route(ShardingRule shardingRule);
    }
}
