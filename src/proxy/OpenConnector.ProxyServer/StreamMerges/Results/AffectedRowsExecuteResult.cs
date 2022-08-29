namespace OpenConnector.ProxyServer.StreamMerges.Results;

public sealed class AffectedRowsExecuteResult:IExecuteResult
{
    
    public int RecordsAffected { get; }
    public long LastInsertId { get; }

    public AffectedRowsExecuteResult(int recordsAffected,long lastInsertId)
    {
        RecordsAffected = recordsAffected;
        LastInsertId = lastInsertId;
    }
}