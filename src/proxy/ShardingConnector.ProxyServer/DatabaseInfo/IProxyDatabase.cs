using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session.Connection.Abstractions;

namespace ShardingConnector.ProxyServer.DatabaseInfo;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}