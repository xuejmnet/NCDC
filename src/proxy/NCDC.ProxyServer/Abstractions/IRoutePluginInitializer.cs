namespace NCDC.ProxyServer.Abstractions;

public interface IRoutePluginInitializer
{
    Task InitializeAsync();
}