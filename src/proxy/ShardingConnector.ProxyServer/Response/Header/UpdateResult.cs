using ShardingConnector.ProxyServer.Abstractions;

namespace ShardingConnector.ProxyServer.Response.Header;

public sealed class UpdateResult:IExecuteResult
{
    public int UpdateCount { get; }
    public long LastInsertId { get; }

    public UpdateResult(int updateCount,long lastInsertId)
    {
        UpdateCount = updateCount;
        LastInsertId = lastInsertId;
    }
}