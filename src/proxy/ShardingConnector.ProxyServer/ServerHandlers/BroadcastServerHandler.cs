using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.ServerHandlers;

/// <summary>
/// 广播到所有的数据源
/// </summary>
public sealed class BroadcastServerHandler:IServerHandler
{
    private readonly ConnectionSession _connectionSession;

    public BroadcastServerHandler(string sql,ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        throw new NotImplementedException();
    }
}