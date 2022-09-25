using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ProxyServer.Abstractions;

public interface IDynamicRouteConfiguration
{
    bool AddRoute(Type routeType);
    bool RemoveRoute(Type routeType);
}