namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppInitializer
{
    Task InitializeAsync();
}