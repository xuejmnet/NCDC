using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;
using NCDC.Sharding.Routes.TableRoutes;
using OpenConnector.Extensions;

namespace NCDC.Sharding.Routes;

public sealed class RouteContextFactory:IRouteContextFactory
{
    private readonly IDataSourceRouteRuleEngine _dataSourceRouteRuleEngine;
    private readonly ITableRouteRuleEngine _tableRouteRuleEngine;

    public RouteContextFactory(IDataSourceRouteRuleEngine dataSourceRouteRuleEngine,ITableRouteRuleEngine tableRouteRuleEngine)
    {
        _dataSourceRouteRuleEngine = dataSourceRouteRuleEngine;
        _tableRouteRuleEngine = tableRouteRuleEngine;
    }
    public RouteContext Create(SqlParserResult sqlParserResult)
    {
        var dataSourceRouteResult = _dataSourceRouteRuleEngine.Route(new DataSourceRouteRuleContext(sqlParserResult));
        var shardingRouteResult = _tableRouteRuleEngine.Route(new TableRouteRuleContext(dataSourceRouteResult,sqlParserResult));
        var routeResult = new RouteResult();
        routeResult.GetRouteUnits().AddAll(shardingRouteResult.RouteUnits);
        return new RouteContext(sqlParserResult.Sql, sqlParserResult.SqlCommandContext,
            sqlParserResult.ParameterContext, routeResult);
    }
}