using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerDataReaderFactory
{
    IServerDataReader Create(string sql,ConnectionSession connectionSession);
}