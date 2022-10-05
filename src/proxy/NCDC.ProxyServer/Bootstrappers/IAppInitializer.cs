using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Bootstrappers;

public interface IAppInitializer
{
    Task<IReadOnlyCollection<IRuntimeContext>> GetDatabases();
    Task<IRuntimeContext> RuntimeContextBuildAsync(string database);
}