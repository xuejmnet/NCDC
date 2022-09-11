using System;
using System.Collections.Generic;
using NCDC.CommandParserBinder.MetaData.Schema;
using OpenConnector.Route.Context;

namespace OpenConnector.Route.Hook
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 14:27:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RoutingHookManager
    {
        private static readonly RoutingHookManager Instance;
        private readonly ICollection<IRoutingHook> routingHooks = NewInstanceServiceLoader.NewServiceInstances<IRoutingHook>();

        private RoutingHookManager()
        {
            
        }
        static RoutingHookManager()
        {
            NewInstanceServiceLoader.Register<IRoutingHook>();
            Instance = new RoutingHookManager();
        }

        public static RoutingHookManager GetInstance()
        {
            return Instance;
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
