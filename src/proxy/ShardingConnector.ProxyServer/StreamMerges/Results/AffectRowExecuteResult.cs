namespace ShardingConnector.ProxyServer.StreamMerges.Results;

public sealed class AffectRowExecuteResult:IExecuteResult
{
    
    public int RecordsAffected { get; }
    public long LastInsertId { get; }

    public AffectRowExecuteResult(int recordsAffected,long lastInsertId)
    {
        RecordsAffected = recordsAffected;
        LastInsertId = lastInsertId;
    }
}