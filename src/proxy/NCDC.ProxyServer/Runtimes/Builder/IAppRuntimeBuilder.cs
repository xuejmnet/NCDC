namespace NCDC.ProxyServer.Runtimes.Builder;

public interface IAppRuntimeBuilder
{
    Task<IRuntimeContext?> BuildAsync(string database);
}