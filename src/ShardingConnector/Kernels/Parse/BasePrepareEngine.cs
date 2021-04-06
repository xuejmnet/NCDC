using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Exceptions;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Kernels.Route.Rule;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Kernels.Parse
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Tuesday, 23 March 2021 21:14:46
    * @Email: 326308290@qq.com
    */
    public abstract class BasePrepareEngine
    {

        private readonly DataNodeRouter _router;
        private readonly ICollection<IBaseRule> _rules;

        protected BasePrepareEngine(DataNodeRouter router, ICollection<IBaseRule> rules)
        {
            _router = router;
            _rules = rules;
        }

        protected abstract IList<object> CloneParameters(IList<object> parameters);
        protected abstract RouteContext Route(DataNodeRouter router,string sql, IList<object> parameters);

        public ExecutionContext<ISqlCommand> Prepare(string sql, IList<object> parameters)
        {
            var cloneParameters = CloneParameters(parameters);
        }

        private RouteContext ExecuteRoute(string sql, List<object> clonedParameters)
        {
            RegisterRouteDecorator();
            return Route(_router, sql, clonedParameters);
        }
        /// <summary>
        /// 注册路由装饰器
        /// </summary>
        private void RegisterRouteDecorator()
        {
            var registeredOrderedAware = OrderedRegistry.GetRegisteredOrderedAware<IRouteDecorator<IBaseRule>>();
            foreach (var routeDecorator in registeredOrderedAware)
            {
                var decorator = CreateRouteDecorator(routeDecorator.GetType());
                var ruleType = decorator.GetGenericType();
                foreach (var rule in _rules.Where(rule=>!rule.GetType().IsAbstract&& ruleType.IsInstanceOfType(rule)))
                {
                    _router.RegisterDecorator(rule,decorator);
                }

            }
        }
        //创建路由装饰器
        private IRouteDecorator<IBaseRule> CreateRouteDecorator(Type routeDecoratorType)
        {
            try
            {
                return (IRouteDecorator<IBaseRule>)Activator.CreateInstance(routeDecoratorType);
            }
            catch (Exception e)
            {
                throw new ShardingException($"Can not find public default constructor for route decorator {routeDecoratorType}", e);
            }
        }
    }
}