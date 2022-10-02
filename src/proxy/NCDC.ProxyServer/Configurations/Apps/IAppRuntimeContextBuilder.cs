using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppRuntimeContextBuilder
{
    Task<IReadOnlyCollection<IRuntimeContext>> BuildAsync();
}