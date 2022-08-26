using ShardingConnector.ProxyServer.Abstractions;

namespace ShardingConnector.ProxyServer.DatabaseInfo;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}