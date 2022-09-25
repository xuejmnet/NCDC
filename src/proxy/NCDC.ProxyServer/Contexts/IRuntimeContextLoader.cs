namespace NCDC.ProxyServer.Contexts;

public interface IRuntimeContextLoader
{
    bool HasLoaded(string databaseName);
    bool Load(IRuntimeContext runtimeContext);
    bool UnLoad(string databaseName);
}