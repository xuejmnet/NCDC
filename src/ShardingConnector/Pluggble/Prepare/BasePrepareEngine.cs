using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Log;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Contexts;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite;
using ShardingConnector.Rewrite.Context;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Pluggble.Prepare
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

        protected BasePrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine)
        {
            _router = new DataNodeRouter(metaData,sqlParserEngine, properties);
            _rules = rules;
            _properties = properties;
        }

        protected abstract List<object> CloneParameters(List<object> parameters);
        protected abstract RouteContext Route(DataNodeRouter router,string sql, List<object> parameters);

        public ExecutionContext Prepare(string sql, List<object> parameters)
        {
            var cloneParameters = CloneParameters(parameters);
            var routeContext = ExecuteRoute(sql, cloneParameters);
            ExecutionContext result = new ExecutionContext(routeContext.GetSqlCommandContext());
            result.GetExecutionUnits().AddAll(ExecuteRewrite(sql, cloneParameters, routeContext));
            if (true)
            {
                SqlLogger.LogSql(sql,false,result.GetSqlStatementContext(),result.GetExecutionUnits());
            }
            return result;
        }


        private ICollection<ExecutionUnit> ExecuteRewrite(string sql, List<object> parameters, RouteContext routeContext)
        {
            RegisterRouteDecorator();
            SqlRewriteContext sqlRewriteContext = _rewriter.CreateSqlRewriteContext(sql, parameters, routeContext.GetSqlCommandContext(), routeContext);
            return !routeContext.GetRouteResult().GetRouteUnits().Any() ? rewrite(sqlRewriteContext) : rewrite(routeContext, sqlRewriteContext);
        }
        private RouteContext ExecuteRoute(string sql, IList<object> clonedParameters)
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
        private ICollection<ExecutionUnit> Rewrite(SqlRewriteContext sqlRewriteContext)
        {
            var sqlRewriteResult = new SqlRewriteEngine().rewrite(sqlRewriteContext);
            String dataSourceName = metaData.getDataSources().getAllInstanceDataSourceNames().iterator().next();
            return Collections.singletonList(new ExecutionUnit(dataSourceName, new SQLUnit(sqlRewriteResult.getSql(), sqlRewriteResult.getParameters())));
        }

        private ICollection<ExecutionUnit> rewrite(RouteContext routeContext, final SQLRewriteContext sqlRewriteContext)
        {
            ICollection<ExecutionUnit> result = new LinkedHashSet<>();
            for (Entry<RouteUnit, SQLRewriteResult> entry : new SQLRouteRewriteEngine().rewrite(sqlRewriteContext, routeContext.getRouteResult()).entrySet())
            {
                result.add(new ExecutionUnit(entry.getKey().getDataSourceMapper().getActualName(), new SQLUnit(entry.getValue().getSql(), entry.getValue().getParameters())));
            }
            return result;
        }
    }
}