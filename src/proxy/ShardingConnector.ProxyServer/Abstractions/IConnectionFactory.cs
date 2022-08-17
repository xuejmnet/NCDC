using ShardingConnector.ProxyServer.Connection;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IConnectionFactory
{
    IServerConnection Create();
}