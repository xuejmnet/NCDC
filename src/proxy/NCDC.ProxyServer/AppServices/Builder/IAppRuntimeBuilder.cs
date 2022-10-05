using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.AppServices.Builder;

public interface IAppRuntimeBuilder
{
    Task<IRuntimeContext> BuildAsync(string database);
}