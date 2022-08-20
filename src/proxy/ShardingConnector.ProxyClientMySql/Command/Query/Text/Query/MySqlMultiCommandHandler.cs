using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;

namespace ShardingConnector.ProxyClientMySql.Command.Query.Text.Query;

public sealed class MySqlMultiCommandHandler:ITextCommandHandler
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IServerResponse Execute()
    {
        throw new NotImplementedException();
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }
}