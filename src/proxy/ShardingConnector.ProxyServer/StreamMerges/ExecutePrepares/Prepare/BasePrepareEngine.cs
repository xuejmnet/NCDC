using ShardingConnector.CommandParser.SqlParseEngines;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor.SqlLog;
using ShardingConnector.Extensions;
using ShardingConnector.ProxyServer.StreamMerges.Executors.Context;
using ShardingConnector.RewriteEngine;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.RewriteEngine.Engine;
using ShardingConnector.Route;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingAdoNet;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.ProxyServer.StreamMerges.ExecutePrepares.Prepare
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Tuesday, 23 March 2021 21:14:46
    * @Email: 326308290@qq.com
    */
    public abstract class BasePrepareEngine
    {
        private readonly ICollection<IBaseRule> _rules;

        private readonly DataNodeRouter _router;
        private readonly SqlRewriteEntry _rewriter;

        private readonly ConfigurationProperties _properties;
        private readonly ShardingConnectorMetaData _metaData;

        protected BasePrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine)
        {
            _router = new DataNodeRouter(metaData,sqlParserEngine, properties);
            _rules = rules;
            _properties = properties;
            _metaData = metaData;
            _rewriter = new SqlRewriteEntry(metaData.Schema, properties);
        }

        protected abstract ParameterContext CloneParameters(ParameterContext parameterContext);
        protected abstract RouteContext Route(DataNodeRouter router,string sql, ParameterContext parameterContext);

        public ShardingExecutionContext Prepare(string sql, ParameterContext parameterContext)
        {
            var cloneParameterContext = CloneParameters(parameterContext);
            var routeContext = ExecuteRoute(sql, cloneParameterContext);
            ShardingExecutionContext result = new ShardingExecutionContext(routeContext.GetSqlCommandContext());
  
            var executionUnits = ExecuteRewrite(sql, cloneParameterContext, routeContext);
            result.GetExecutionUnits().AddAll(executionUnits);
            if (true)
            {
                SqlLogger.LogSql(sql,false,result.GetSqlCommandContext(),result.GetExecutionUnits());
            }
            return result;
        }


        private ICollection<ExecutionUnit> ExecuteRewrite(string sql, ParameterContext parameterContext, RouteContext routeContext)
        {
            RegisterRewriteDecorator();
            SqlRewriteContext sqlRewriteContext = _rewriter.CreateSqlRewriteContext(sql, parameterContext, routeContext.GetSqlCommandContext(), routeContext);
            return !routeContext.GetRouteResult().GetRouteUnits().Any() ? Rewrite(sqlRewriteContext) : Rewrite(routeContext, sqlRewriteContext);
        }
        private RouteContext ExecuteRoute(string sql, ParameterContext cloneParameterContext)
        {
            RegisterRouteDecorator();
            return Route(_router, sql, cloneParameterContext);
        }
        /// <summary>
        /// 注册路由装饰器
        /// </summary>
        private void RegisterRouteDecorator()
        {
            var registeredOrderedAware = OrderedRegistry.GetRegisteredOrderedAware<IRouteDecorator>();
            foreach (var routeDecorator in registeredOrderedAware)
            {
                // var decorator = CreateRouteDecorator(routeDecorator.GetType());
                var ruleType = routeDecorator.GetGenericType();
                foreach (var rule in _rules.Where(rule=>!rule.GetType().IsAbstract&& ruleType.IsInstanceOfType(rule)))
                {
                    _router.RegisterDecorator(rule,routeDecorator);
                }

            }
        }
        // //创建路由装饰器
        //  private IRouteDecorator CreateRouteDecorator(Type routeDecoratorType)
        //  {
        //      try
        //      {
        //          return (IRouteDecorator)Activator.CreateInstance(routeDecoratorType);
        //      }
        //      catch (Exception e)
        //      {
        //          throw new ShardingException($"Can not find public default constructor for route decorator {routeDecoratorType}", e);
        //      }
        //  }
        private void RegisterRewriteDecorator()
         {
            var rewriteContextDecorators = OrderedRegistry.GetRegisteredOrderedAware<ISqlRewriteContextDecorator>();

            foreach (var rewriteContextDecorator in rewriteContextDecorators)
            {
                // var decorator = CreateRewriteDecorator(orderAware.GetType());
                var ruleType = rewriteContextDecorator.GetGenericType();
                foreach (var rule in _rules.Where(rule => !rule.GetType().IsAbstract && ruleType.IsInstanceOfType(rule)))
                {
                    _rewriter.RegisterDecorator(rule, rewriteContextDecorator);
                }
            }
        }

        // private ISqlRewriteContextDecorator CreateRewriteDecorator(Type rewriteDecorator)
        // {
        //     try
        //     {
        //         return (ISqlRewriteContextDecorator)Activator.CreateInstance(rewriteDecorator); ;
        //     }
        //     catch (Exception ex) {
        //         throw new ShardingException($"Can not find public default constructor for rewrite decorator `{rewriteDecorator}`", ex);
        //     }
        // }
        private ICollection<ExecutionUnit> Rewrite(SqlRewriteContext sqlRewriteContext)
        {
            var sqlRewriteResult = new SqlRewriteEngine().Rewrite(sqlRewriteContext);
            String dataSourceName = _metaData.DataSources.GetAllInstanceDataSourceNames().First();
            return new List<ExecutionUnit>(){new ExecutionUnit(dataSourceName, new SqlUnit(sqlRewriteResult.Sql, sqlRewriteResult.ParameterContext))};
        }

        private ICollection<ExecutionUnit> Rewrite(RouteContext routeContext, SqlRewriteContext sqlRewriteContext)
        {
            ICollection<ExecutionUnit> result = new LinkedList<ExecutionUnit>();
            var sqlRewriteResults = new SqlRouteRewriteEngine().Rewrite(sqlRewriteContext, routeContext.GetRouteResult());
            foreach (var sqlRewriteResult in sqlRewriteResults)
            {
                result.Add(new ExecutionUnit(sqlRewriteResult.Key.DataSourceMapper.ActualName, new SqlUnit(sqlRewriteResult.Value.Sql, sqlRewriteResult.Value.ParameterContext)));
            }
            return result;
        }
    }
}