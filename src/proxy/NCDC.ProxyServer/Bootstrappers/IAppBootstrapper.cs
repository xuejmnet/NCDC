namespace NCDC.ProxyServer.Bootstrappers;

public interface IAppBootstrapper
{
    Task StartAsync();
    // Task StopAsync();
}