namespace NCDC.ProxyServer.Abstractions;

public interface IRouteConfiguration
{
   bool HasRoute(string routeIdentity);
   Type GetRoute(string routeIdentity);
   IReadOnlyCollection<string> GetRouteIdentities();
}