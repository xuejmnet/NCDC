using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Configurations.Initializers;

namespace NCDC.ProxyServer;

public sealed class DefaultInitializerManager:IInitializerManager
{
    private readonly IAppUserInitializer _appUserInitializer;
    private readonly IRoutePluginInitializer _routePluginInitializer;
    private readonly IAppInitializer _appInitializer;

    public DefaultInitializerManager(IAppUserInitializer appUserInitializer,IRoutePluginInitializer routePluginInitializer,IAppInitializer appInitializer)
    {
        _appUserInitializer = appUserInitializer;
        _routePluginInitializer = routePluginInitializer;
        _appInitializer = appInitializer;
    }
    public async Task InitializeAsync()
    {
        await _appUserInitializer.InitializeAsync();
        await _routePluginInitializer.InitializeAsync();
        await _appInitializer.InitializeAsync();
    }
}