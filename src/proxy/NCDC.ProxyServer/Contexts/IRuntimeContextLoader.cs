using NCDC.ProxyServer.Contexts.RuntimeContextBuilders;

namespace NCDC.ProxyServer.Contexts;

public interface IRuntimeContextLoader
{
    bool HasLoaded(string databaseName);
    Task LoadAsync(string databaseName);
    Task UnLoadAsync(string databaseName);
}