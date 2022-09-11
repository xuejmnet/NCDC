namespace NCDC.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRouteRuleEngine
{
    DataSourceRouteResult Route(DataSourceRouteRuleContext context);
}