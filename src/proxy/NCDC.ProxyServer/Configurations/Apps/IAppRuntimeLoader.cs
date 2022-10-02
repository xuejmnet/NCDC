using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppRuntimeLoader
{
    bool HasLoaded(string databaseName);
    bool Load(IRuntimeContext runtimeContext);
    bool UnLoad(string databaseName);
}