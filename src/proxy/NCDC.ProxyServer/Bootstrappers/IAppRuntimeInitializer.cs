namespace NCDC.ProxyServer.Bootstrappers;

public interface IAppRuntimeInitializer
{
    Task InitializeAsync();
}