using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.Query;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface ITextCommandExecutor
{
    List<QueryResponse> ExecuteQuery(bool serial, ExecutionContext executionContext);
    List<int> ExecuteNonQuery(bool serial,ExecutionContext executionContext);
}