using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.ServerHandlers.Results;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class SkipServerHandler:IServerHandler
{
    public IServerResult Execute()
    {
        return new RecordsAffectedServerResult();
    }
}