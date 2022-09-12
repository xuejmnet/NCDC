namespace NCDC.Host;

public interface IServiceHost
{
    Task StartAsync();
    Task StopAsync();
}