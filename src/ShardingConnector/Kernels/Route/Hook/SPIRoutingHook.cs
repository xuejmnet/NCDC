using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.MetaData.Schema;

namespace ShardingConnector.Kernels.Route.Hook
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 15:46:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SPIRoutingHook : IRoutingHook
    {
        public void Start(string sql)
        {
            Console.WriteLine("Start");
        }

        public void FinishSuccess(RouteContext routeContext, SchemaMetaData schemaMetaData)
        {
            Console.WriteLine("FinishSuccess");
        }

        public void FinishFailure(Exception cause)
        {
            Console.WriteLine("FinishFailure");
        }
    }
}
