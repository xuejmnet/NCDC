using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

/// <summary>
/// 通用的数据库文本命令
/// </summary>
public sealed class GenericDatabaseTextCommandHandler:ITextCommandHandler
{
    public IServerResponse Execute()
    {
        throw new NotImplementedException();
    }
}