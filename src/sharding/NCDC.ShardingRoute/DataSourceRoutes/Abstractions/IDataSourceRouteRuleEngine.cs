using NCDC.Plugin;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public interface IDataSourceRouteRuleEngine
{
    DataSourceRouteResult Route(DataSourceRouteRuleContext context);
}