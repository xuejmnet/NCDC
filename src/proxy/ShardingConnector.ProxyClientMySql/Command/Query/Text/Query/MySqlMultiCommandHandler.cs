using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyClientMySql.Command.Query.Text.Query;

public sealed class MySqlMultiCommandHandler:ITextProtocolHandler
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IResponseHeader Execute()
    {
        throw new NotImplementedException();
    }

    public bool Next()
    {
        throw new NotImplementedException();
    }
}