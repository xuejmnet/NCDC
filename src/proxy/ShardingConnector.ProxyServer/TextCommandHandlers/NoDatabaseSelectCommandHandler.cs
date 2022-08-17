using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public sealed class NoDatabaseSelectCommandHandler:ITextCommandHandler
{
    public IResponseHeader Execute()
    {
        return new UpdateResponseHeader();
    }
}