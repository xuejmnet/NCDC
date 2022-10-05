using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Bootstrappers;

public class AppBootstrapper:IAppBootstrapper
{
    private readonly AppServices.IAppRuntimeInitializer _appRuntimeInitializer;

    public AppBootstrapper(AppServices.IAppRuntimeInitializer appRuntimeInitializer)
    {
        _appRuntimeInitializer = appRuntimeInitializer;
    }
    public async Task StartAsync()
    {
        await _appRuntimeInitializer.InitializeAsync();
    }
}