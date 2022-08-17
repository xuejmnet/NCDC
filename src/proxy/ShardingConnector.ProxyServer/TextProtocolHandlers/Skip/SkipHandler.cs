using ShardingConnector.CommandParser.Command;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyServer.TextProtocolHandlers.Skip;

public sealed class SkipHandler:ITextProtocolHandler
{
    private readonly ISqlCommand _sqlCommand;

    public SkipHandler(ISqlCommand sqlCommand)
    {
        _sqlCommand = sqlCommand;
    }
    public IResponseHeader Execute()
    {
        return new UpdateResponseHeader();
    }
}