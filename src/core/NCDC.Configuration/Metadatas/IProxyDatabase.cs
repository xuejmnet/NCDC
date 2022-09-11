using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace NCDC.Configuration.Metadatas;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}