namespace ShardingConnector.ProxyServer;

public interface IProxyRuntimeContext
{
    bool DatabaseExists(string database);
}