namespace NCDC.ProxyServer.Contexts.Initializers;

public interface IRuntimeContextInitializer
{
    Task InitializeAsync();
}