using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Routes;

public sealed class RouteContext
{
    private readonly string _sql;
    private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
    private readonly ParameterContext _parameterContext;
    private readonly RouteResult _routeResult;

    public RouteContext(string sql,ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext, RouteResult routeResult)
    {
        _sql = sql;
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

    public string GetSql()
    {
        return _sql;
    }
}