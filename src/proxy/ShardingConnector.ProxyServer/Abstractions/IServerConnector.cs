using ShardingConnector.ProxyServer.Binaries;

namespace ShardingConnector.ProxyServer.Abstractions;

/// <summary>
/// 服务端的链接器
/// </summary>
public interface IServerConnector
{
    IServerResult Execute();
    bool Read();
    BinaryRow GetQueryRow();
}