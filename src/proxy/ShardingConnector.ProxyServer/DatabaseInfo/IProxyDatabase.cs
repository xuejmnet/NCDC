using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.ProxyAdoNets;
using ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

namespace ShardingConnector.ProxyServer.DatabaseInfo;

public interface IProxyDatabase
{
    string DataSourceName { get; }
    bool IsDefault { get; }
    IServerDbConnection CreateServerDbConnection();
}