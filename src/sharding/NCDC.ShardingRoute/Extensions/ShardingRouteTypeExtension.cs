using NCDC.Plugin;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ShardingRoute.Extensions;

public static class ShardingRouteTypeExtension
{
    private const string UN_KNOWN_ROUTE_TYPE_NAME = "UnKnownRouteTypeName";
    /// <summary>
    /// 获取路由名称fullname为null时返回UnKnownRouteTypeName
    /// </summary>
    /// <param name="routeType"></param>
    /// <returns></returns>
    public static string GetRouteTypeFullName(this Type routeType)
    {
        return routeType.FullName ?? UN_KNOWN_ROUTE_TYPE_NAME;
    }

    public static bool IsRoute(this Type type)
    {
        return typeof(IRoute).IsAssignableFrom(type);
    }
    public static bool IsTableRoute(this Type type)
    {
        return type.IsRoute()&&typeof(ITableRoute).IsAssignableFrom(type);
    }
    public static bool IsDataSourceRoute(this Type type)
    {
        return type.IsRoute()&&typeof(IDataSourceRoute).IsAssignableFrom(type);
    }
}