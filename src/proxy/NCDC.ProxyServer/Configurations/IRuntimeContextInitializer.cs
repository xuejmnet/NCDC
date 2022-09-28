namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextInitializer
{
    Task InitializeAsync();
}