namespace OpenConnector.Host;

public interface IServiceHost
{
    Task StartAsync();
    Task StopAsync();
}