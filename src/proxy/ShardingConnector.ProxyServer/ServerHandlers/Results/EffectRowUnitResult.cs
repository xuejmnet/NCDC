namespace ShardingConnector.ProxyServer.ServerHandlers.Results;

public sealed class EffectRowUnitResult
{
    public int UpdateCount { get; }
    public long LastInsertId { get; }

    public EffectRowUnitResult(int updateCount,long lastInsertId)
    {
        UpdateCount = updateCount;
        LastInsertId = lastInsertId;
    }
}