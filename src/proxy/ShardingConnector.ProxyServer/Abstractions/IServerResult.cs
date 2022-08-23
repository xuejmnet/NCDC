using ShardingConnector.ProxyServer.Commons;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerResult
{
    ResultTypeEnum ResultType { get; }
}