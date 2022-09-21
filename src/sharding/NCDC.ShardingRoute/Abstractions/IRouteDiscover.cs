namespace NCDC.ShardingRoute.Abstractions;

public interface IRouteDiscover
{
    /// <summary>
    /// 路由的唯一标识
    /// </summary>
    string RouteIdentity { get; }
}