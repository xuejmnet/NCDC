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
        private readonly ICollection<IRoutingHook> routingHooks = NewInstanceServiceLoader.NewServiceInstances<IRoutingHook>();

        static SPIRoutingHook()
        {
            NewInstanceServiceLoader.Register<IRoutingHook>();
        }
        public void Start(string sql)
        {
            foreach (var routingHook in routingHooks)
            {
                routingHook.Start(sql);
            }
        }

        public void FinishSuccess(RouteContext routeContext, SchemaMetaData schemaMetaData)
        {
            foreach (var routingHook in routingHooks)
            {
                routingHook.FinishSuccess(routeContext, schemaMetaData);
            }
        }

        public void FinishFailure(Exception cause)
        {
            foreach (var routingHook in routingHooks)
            {
                routingHook.FinishFailure(cause);
            }
        }
    }
}
