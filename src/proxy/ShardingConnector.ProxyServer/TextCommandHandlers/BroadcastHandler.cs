using ShardingConnector.CommandParser.Command;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.TextProtocolHandlers;

public sealed class BroadcastHandler:ITextCommandHandler
{
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;

    public BroadcastHandler(string sql,ConnectionSession connectionSession)
    {
        _sql = sql;
        _connectionSession = connectionSession;
    }
    public IResponseHeader Execute()
    {
        var responseHeaders = new List<IResponseHeader>();
        // _connectionSession.getda
        // _connectionSession.
        return new UpdateResponseHeader();
    }
}