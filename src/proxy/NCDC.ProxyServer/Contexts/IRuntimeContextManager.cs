namespace NCDC.ProxyServer.Contexts;

public interface IRuntimeContextManager
{
    bool AddRuntimeContext(string databaseName);
    IRuntimeContext GetRuntimeContext(string databaseName);
    bool HasRuntimeContext(string databaseName);
}