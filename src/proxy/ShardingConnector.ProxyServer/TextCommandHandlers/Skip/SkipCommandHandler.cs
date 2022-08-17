using ShardingConnector.CommandParser.Command;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyServer.TextCommandHandlers.Skip;

public sealed class SkipCommandHandler:ITextCommandHandler
{
    private readonly ISqlCommand _sqlCommand;

    public SkipCommandHandler(ISqlCommand sqlCommand)
    {
        _sqlCommand = sqlCommand;
    }
    public IResponseHeader Execute()
    {
        return new UpdateResponseHeader();
    }
}