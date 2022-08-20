using ShardingConnector.ProxyServer.Abstractions;

namespace ShardingConnector.ProxyServer.Response.EffectRow;

public sealed class EffectRowServerExecuteResult:IServerExecuteResult
{
    public int UpdateCount { get; }
    public long LastInsertId { get; }

    public EffectRowServerExecuteResult(int updateCount,long lastInsertId)
    {
        UpdateCount = updateCount;
        LastInsertId = lastInsertId;
    }
}