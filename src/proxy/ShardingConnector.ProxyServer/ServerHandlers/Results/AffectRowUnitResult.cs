namespace ShardingConnector.ProxyServer.ServerHandlers.Results;

public sealed class AffectRowUnitResult
{
    public int UpdateCount { get; }
    public long LastInsertId { get; }

    public AffectRowUnitResult(int updateCount,long lastInsertId)
    {
        UpdateCount = updateCount;
        LastInsertId = lastInsertId;
    }
}