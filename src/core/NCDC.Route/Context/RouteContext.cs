using System.Collections.Generic;
using System.Data.Common;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParserBinder.Command;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Route.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/30 12:53:36
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RouteContext
    {
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
        private readonly ParameterContext _parameterContext;
        private readonly RouteResult _routeResult;

        public RouteContext(ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext, RouteResult routeResult)
        {
            _sqlCommandContext = sqlCommandContext;
            _parameterContext = parameterContext;
            _routeResult = routeResult;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
        {
            return _sqlCommandContext;
        }

        public ParameterContext GetParameterContext()
        {
            return _parameterContext;
        }

        public RouteResult GetRouteResult()
        {
            return _routeResult;
        }
    }
}
