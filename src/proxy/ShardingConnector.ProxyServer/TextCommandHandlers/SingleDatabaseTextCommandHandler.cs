using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public sealed class SingleDatabaseTextCommandHandler:ITextCommandHandler
{
    public IResponseHeader Execute()
    {
        throw new NotImplementedException();
    }
}