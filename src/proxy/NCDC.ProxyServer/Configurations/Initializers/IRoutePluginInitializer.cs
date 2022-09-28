namespace NCDC.ProxyServer.Configurations.Initializers;

public interface IRoutePluginInitializer
{
    Task InitializeAsync();
}