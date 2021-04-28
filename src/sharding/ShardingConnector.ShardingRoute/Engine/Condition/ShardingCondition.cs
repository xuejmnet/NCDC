using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace ShardingConnector.ShardingRoute.Engine.Condition
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:44:52
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingCondition
    {
        public readonly ICollection<IRouteValue> RouteValues = new LinkedList<IRouteValue>();
    }
}
