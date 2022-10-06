using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Configurations.Initializers;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Bootstrappers;

public class AppBootstrapper:IAppBootstrapper
{
    private readonly IRoutePluginInitializer _routePluginInitializer;
    private readonly IAppInitializer _appInitializer;

    public AppBootstrapper(IRoutePluginInitializer routePluginInitializer,IAppInitializer appInitializer)
    {
        _routePluginInitializer = routePluginInitializer;
        _appInitializer = appInitializer;
    }
    public async Task StartAsync()
    {
       await _routePluginInitializer.InitializeAsync();
        await _appInitializer.InitializeAsync();
    }
}