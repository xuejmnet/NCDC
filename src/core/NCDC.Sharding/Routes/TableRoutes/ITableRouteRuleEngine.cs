namespace NCDC.Sharding.Routes.TableRoutes;

public interface ITableRouteRuleEngine
{
    ShardingRouteResult Route(TableRouteRuleContext context);
}