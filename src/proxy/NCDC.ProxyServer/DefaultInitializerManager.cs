using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyServer;

public sealed class DefaultInitializerManager:IInitializerManager
{
    private readonly IRoutePluginInitializer _routePluginInitializer;
    private readonly IAppInitializer _appInitializer;

    public DefaultInitializerManager(IRoutePluginInitializer routePluginInitializer,IAppInitializer appInitializer)
    {
        _routePluginInitializer = routePluginInitializer;
        _appInitializer = appInitializer;
    }
    public async Task InitializeAsync()
    {
        await _routePluginInitializer.InitializeAsync();
        await _appInitializer.InitializeAsync();
    }
}