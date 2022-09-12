using Microsoft.Extensions.DependencyInjection;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ShardingRoute;

public static class NCDCShardingRouteExtension
{
    public static IServiceCollection AddShardingRoute(this IServiceCollection services)
    {
        services.AddSingleton<IDataSourceRouteRuleEngine, DataSourceRouteRuleEngine>();
        services.AddSingleton<IDataSourceRouteManager, DataSourceRouteManager>();
        services.AddSingleton<ITableRouteRuleEngine, TableRouteRuleEngine>();
        services.AddSingleton<ITableRouteManager, TableRouteManager>();
        services.AddSingleton<IRouteContextFactory, RouteContextFactory>();
        return services;
    }
}