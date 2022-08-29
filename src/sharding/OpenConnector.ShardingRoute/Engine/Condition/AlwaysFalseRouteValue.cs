using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace OpenConnector.ShardingRoute.Engine.Condition
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 12:05:54
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AlwaysFalseRouteValue: IRouteValue
    {
        public string GetColumnName()
        {
            return string.Empty;
        }

        public string GetTableName()
        {
            return string.Empty;
        }
    }
}
