using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace OpenConnector.ProxyServer.DatabaseInfo;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}