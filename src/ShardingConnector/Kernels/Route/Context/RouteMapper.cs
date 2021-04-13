using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Kernels.Route.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 17:11:55
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RouteMapper
    {
        public RouteMapper(string logicName, string actualName)
        {
            LogicName = logicName;
            ActualName = actualName;
        }
        /// <summary>
        /// 逻辑名称
        /// </summary>
        public  string LogicName { get; }
        /// <summary>
        /// 真实名称
        /// </summary>
        public  string ActualName { get; }
    }
}
