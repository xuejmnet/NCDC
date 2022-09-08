using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes;

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