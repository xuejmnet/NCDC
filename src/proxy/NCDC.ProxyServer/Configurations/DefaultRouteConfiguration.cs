// using System.Collections.Concurrent;
// using System.Collections.Immutable;
// using NCDC.Exceptions;
// using NCDC.Plugin;
// using NCDC.Plugin.Extensions;
// using NCDC.ProxyServer.Abstractions;
// using NCDC.ShardingRoute.Abstractions;
// using NCDC.ShardingRoute.Extensions;
//
// namespace NCDC.ProxyServer.Configurations;
//
// public class DefaultRouteConfiguration:IRouteConfiguration,IDynamicRouteConfiguration
// {
//     /// <summary>
//     /// key:route type full name
//     /// value:route type
//     /// </summary>
//     private readonly ConcurrentDictionary<string, Type> _routes = new();
//
//     public bool HasRoute(string routeIdentity)
//     {
//         return _routes.ContainsKey(routeIdentity);
//     }
//
//     public Type GetRoute(string routeIdentity)
//     {
//         if (!_routes.TryGetValue(routeIdentity, out var routeType))
//         {
//             throw new ShardingException($"route identity:[{routeIdentity}] not found");
//         }
//
//         return routeType;
//     }
//
//     public IReadOnlyCollection<string> GetRouteIdentities()
//     {
//         return _routes.Keys.ToImmutableHashSet();
//     }
//     public bool AddRoute(Type routeType)
//     {
//         CheckIsRouteRuleType(routeType);
//         var routeIdentity = routeType.GetRouteRuleTypeFullName();
//         if (HasRoute(routeIdentity))
//         {
//             return false;
//         }
//
//         return _routes.TryAdd(routeIdentity, routeType);
//     }
//
//     public bool RemoveRoute(Type routeType)
//     {
//         CheckIsRouteRuleType(routeType);
//         var routeIdentity = routeType.GetRouteRuleTypeFullName();
//         if (!HasRoute(routeIdentity))
//         {
//             return false;
//         }
//
//         return _routes.Remove(routeIdentity, out _);
//     }
//
//     private void CheckIsRouteRuleType(Type routeType)
//     {
//         if (!routeType.IsRouteRule())
//         {
//             throw new ShardingException($"{routeType} must implement {nameof(IRouteRule)}");
//         }
//     }
//
// }