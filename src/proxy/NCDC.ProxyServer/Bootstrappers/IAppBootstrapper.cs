namespace NCDC.ProxyServer.Bootstrappers;

public interface IAppBootstrapper
{
    Task StartAsync(CancellationToken cancellationToken=default);
    Task StopAsync(CancellationToken cancellationToken=default);
}