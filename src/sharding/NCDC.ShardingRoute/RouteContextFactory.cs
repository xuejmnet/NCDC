using NCDC.Basic.Configurations;
using NCDC.CommandParser.Command.DML;
using NCDC.Extensions;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes;
using NCDC.ShardingRoute.TableRoutes.Abstractions;
using NCDC.Extensions;

namespace NCDC.ShardingRoute;

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
        var shardingRouteResult = _tableRouteRuleEngine.Route(new TableRouteContext(dataSourceRouteResult,sqlParserResult));
        var routeResult = new RouteResult();
        routeResult.GetRouteUnits().AddAll(shardingRouteResult.RouteUnits);
        return new RouteContext(sqlParserResult.Sql, sqlParserResult.SqlCommandContext,
            sqlParserResult.ParameterContext, routeResult);
    }
}