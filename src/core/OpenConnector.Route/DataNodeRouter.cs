using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.CommandParserBinder;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.MetaData;
using OpenConnector.Common.Rule;
using OpenConnector.Route.Context;
using OpenConnector.Route.Hook;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Route
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
        private readonly OpenConnectorMetaData _metaData;
        private readonly ConfigurationProperties _properties;
        private readonly ISqlCommandParser _commandParser;
        private readonly IDictionary<IBaseRule, IRouteDecorator> _decorators =
            new Dictionary<IBaseRule, IRouteDecorator>();


        public DataNodeRouter(OpenConnectorMetaData metaData, ISqlCommandParser commandParser, ConfigurationProperties properties)
        {
            _metaData = metaData;
            _commandParser = commandParser;
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
            var sqlCommand = _commandParser.Parse(sql, useCache);
            // try
            // {
            //     
                ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_metaData.Schema, sql, parameterContext, sqlCommand);
                return new RouteContext(sqlCommandContext, parameterContext, new RouteResult());
            //     // TODO should pass parameters for master-slave
            // }
            // catch (IndexOutOfRangeException ex)
            // {
            //     return new RouteContext(new GenericSqlCommandContext<ISqlCommand>(sqlCommand), parameterContext, new RouteResult());
            // }
        }
    }
}
