using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;

namespace ShardingConnector.Route.Context
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
        private readonly IDictionary<string, DbParameter> _parameters;
        private readonly RouteResult _routeResult;

        public RouteContext(ISqlCommandContext<ISqlCommand> sqlCommandContext, IDictionary<string, DbParameter> parameters, RouteResult routeResult)
        {
            _sqlCommandContext = sqlCommandContext;
            _parameters = parameters;
            _routeResult = routeResult;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
        {
            return _sqlCommandContext;
        }

        public IDictionary<string, DbParameter> GetParameters()
        {
            return _parameters;
        }

        public RouteResult GetRouteResult()
        {
            return _routeResult;
        }
    }
}
