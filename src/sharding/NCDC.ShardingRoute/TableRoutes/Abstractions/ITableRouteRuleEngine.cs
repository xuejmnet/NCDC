namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public interface ITableRouteRuleEngine
{
    ShardingRouteResult Route(TableRouteContext context);
}