using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserBinder;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserEngine;
using ShardingConnector.Route.Context;
using ShardingConnector.Route.Hook;
using ShardingConnector.ShardingAdoNet;

namespace ShardingConnector.Route
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
        private readonly IDictionary<IBaseRule, IRouteDecorator> _decorators =
            new Dictionary<IBaseRule, IRouteDecorator>();


        public DataNodeRouter(ShardingConnectorMetaData metaData, SqlParserEngine parserEngine, ConfigurationProperties properties)
        {
            _metaData = metaData;
            _parserEngine = parserEngine;
            _properties = properties;
        }
        public void RegisterDecorator(IBaseRule rule, IRouteDecorator decorator)
        {
            _decorators.Add(rule, decorator);
        }

        public RouteContext Route(string sql, ParameterContext parameterContext, bool useCache)
        {
            var routingHookManager = RoutingHookManager.GetInstance();
            routingHookManager.Start(sql);
            try
            {
                RouteContext result = ExecuteRoute(sql, parameterContext, useCache);
                routingHookManager.FinishSuccess(result, _metaData.Schema);
                return result;
                // CHECKSTYLE:OFF
            }
            catch (Exception ex)
            {
                // CHECKSTYLE:ON
                routingHookManager.FinishFailure(ex);
                throw;
            }

        }

        private RouteContext ExecuteRoute(string sql, ParameterContext parameterContext, bool useCache)
        {
            var result = CreateRouteContext(sql, parameterContext, useCache);
            foreach (var decorator in _decorators)
            {
                result = decorator.Value.Decorate(result, _metaData, decorator.Key, _properties);
            }
            return result;
        }

        private RouteContext CreateRouteContext(string sql, ParameterContext parameterContext, bool useCache)
        {
            var sqlCommand = _parserEngine.Parse(sql, useCache);
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
