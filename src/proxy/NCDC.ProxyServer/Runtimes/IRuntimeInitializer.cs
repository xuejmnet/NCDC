namespace NCDC.ProxyServer.Runtimes;

public interface IRuntimeInitializer
{
    Task InitializeAsync();
}