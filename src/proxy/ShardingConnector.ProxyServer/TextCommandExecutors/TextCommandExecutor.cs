using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.Query;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.ProxyServer.TextCommandExecutors;

public sealed class TextCommandExecutor:ITextCommandExecutor
{
    public List<QueryResponse> ExecuteQuery(bool serial, ExecutionContext executionContext)
    {
        throw new NotImplementedException();
    }

    public List<int> ExecuteNonQuery(bool serial, ExecutionContext executionContext)
    {
        throw new NotImplementedException();
    }
}