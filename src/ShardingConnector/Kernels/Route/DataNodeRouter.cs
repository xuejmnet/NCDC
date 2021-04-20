using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Kernels.Route.Hook;
using ShardingConnector.Parser.Binder;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Kernels.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/30 13:00:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataNodeRouter
    {
        private readonly ShardingConnectorMetaData _metaData;
        private readonly ConfigurationProperties _properties;
        private readonly SqlParserEngine _parserEngine;
        private readonly IDictionary<IBaseRule, IRouteDecorator<IBaseRule>> _decorators =
            new Dictionary<IBaseRule, IRouteDecorator<IBaseRule>>();

        private readonly RoutingHookManager _routingHook = new RoutingHookManager();

        public DataNodeRouter(ShardingConnectorMetaData metaData, SqlParserEngine parserEngine, ConfigurationProperties properties)
        {
            _metaData = metaData;
            _parserEngine = parserEngine;
            _properties = properties;
        }
        public void RegisterDecorator(IBaseRule rule, IRouteDecorator<IBaseRule> decorator)
        {
            _decorators.Add(rule, decorator);
        }

        public RouteContext Route(string sql, List<object> parameters)
        {
            _routingHook.Start(sql);
            try
            {
                RouteContext result = ExecuteRoute(sql, parameters);
                _routingHook.FinishSuccess(result, _metaData.Schema);
                return result;
                // CHECKSTYLE:OFF
            }
            catch (Exception ex)
            {
                // CHECKSTYLE:ON
                _routingHook.FinishFailure(ex);
                throw ex;
            }

        }

        private RouteContext ExecuteRoute(string sql, List<object> parameters)
        {
            var result = CreateRouteContext(sql, parameters);
            foreach (var decorator in _decorators)
            {
                result = decorator.Value.Decorate(result, _metaData, decorator.Key, _properties);
            }
            return result;
        }

        private RouteContext CreateRouteContext(string sql, List<object> parameters)
        {
            var sqlCommand = _parserEngine.Parse(sql);
            try
            {
                ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.NewInstance(_metaData.Schema, sql, parameters, sqlCommand);
                return new RouteContext(sqlCommandContext, parameters, new RouteResult());
                // TODO should pass parameters for master-slave
            }
            catch (IndexOutOfRangeException ex)
            {
                return new RouteContext(new GenericSqlCommandContext<ISqlCommand>(sqlCommand), parameters, new RouteResult());
            }
        }
    }
}
