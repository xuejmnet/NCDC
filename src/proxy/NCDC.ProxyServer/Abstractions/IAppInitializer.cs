namespace NCDC.ProxyServer.Abstractions;

public interface IAppInitializer
{
    Task InitializeAsync();
}