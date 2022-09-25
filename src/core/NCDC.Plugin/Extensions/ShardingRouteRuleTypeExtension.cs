using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.TableRouteRules;

namespace NCDC.Plugin.Extensions;

public static class ShardingRouteRuleTypeExtension
{
    
    private const string UN_KNOWN_ROUTE_TYPE_NAME = "UnKnownRouteRuleTypeName";
    /// <summary>
    /// 获取路由名称fullname为null时返回UnKnownRouteTypeName
    /// </summary>
    /// <param name="routeType"></param>
    /// <returns></returns>
    public static string GetRouteRuleTypeFullName(this Type routeType)
    {
        return routeType.FullName ?? UN_KNOWN_ROUTE_TYPE_NAME;
    }

    public static bool IsRouteRule(this Type type)
    {
        return typeof(IRouteRule).IsAssignableFrom(type);
    }
    public static bool IsTableRouteRule(this Type type)
    {
        return type.IsRouteRule()&&typeof(ITableRouteRule).IsAssignableFrom(type);
    }
    public static bool IsDataSourceRouteRule(this Type type)
    {
        return type.IsRouteRule()&&typeof(IDataSourceRouteRule).IsAssignableFrom(type);
    }
}