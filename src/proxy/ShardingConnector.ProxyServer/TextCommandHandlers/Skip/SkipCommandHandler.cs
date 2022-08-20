using ShardingConnector.CommandParser.Command;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.EffectRow;

namespace ShardingConnector.ProxyServer.TextCommandHandlers.Skip;

public sealed class SkipCommandHandler:ITextCommandHandler
{
    private readonly ISqlCommand _sqlCommand;

    public SkipCommandHandler(ISqlCommand sqlCommand)
    {
        _sqlCommand = sqlCommand;
    }
    public IServerResponse Execute()
    {
        return new EffectRowServerResponse();
    }
}