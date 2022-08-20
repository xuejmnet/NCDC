using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.EffectRow;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

/// <summary>
/// 没有选中database的情况下的文本命令
/// </summary>
public sealed class NoDatabaseTextCommandHandler:ITextCommandHandler
{
    public IServerResponse Execute()
    {
        return new EffectRowServerResponse();
    }
}