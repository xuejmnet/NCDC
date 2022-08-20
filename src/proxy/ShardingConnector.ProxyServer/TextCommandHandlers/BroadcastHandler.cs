using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.EffectRow;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public sealed class BroadcastHandler:ITextCommandHandler
{
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;

    public BroadcastHandler(string sql,ConnectionSession connectionSession)
    {
        _sql = sql;
        _connectionSession = connectionSession;
    }
    public IServerResponse Execute()
    {
        var responseHeaders = new List<IServerResponse>();
        // _connectionSession.getda
        // _connectionSession.
        return new EffectRowServerResponse();
    }
}