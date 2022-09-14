using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection.Metadatas;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}