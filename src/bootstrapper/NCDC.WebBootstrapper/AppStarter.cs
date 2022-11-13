using NCDC.Logger;
using NCDC.ProxyServer.Bootstrappers;

namespace NCDC.WebBootstrapper;

public class AppStarter:BackgroundService
{
    private readonly IAppBootstrapper _appBootstrapper;

    public AppStarter(IServiceProvider serviceProvider,ILoggerFactory loggerFactory)
    {
        NCDCLoggerFactory.DefaultFactory =loggerFactory;
        _appBootstrapper = serviceProvider.GetRequiredService<IAppBootstrapper>();
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _appBootstrapper.StartAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _appBootstrapper.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}