namespace ShardingConnector.ProxyServer.Response.EffectRow;

public sealed class EffectRowServerResponse:IServerResponse
{
    public long LastInsertId { get; }
    public List<int> UpdateCounts { get; }
    private long _updateCount;

    public EffectRowServerResponse():this(new List<EffectRowServerExecuteResult>(0))
    {
        
    }

    public long GetUpdateCount()
    {
        return _updateCount;
    }

    public EffectRowServerResponse(List<EffectRowServerExecuteResult> updateResults)
    {
        UpdateCounts = new List<int>(updateResults.Count);
        long lastInsertId = 0;
        for (var i = 0; i < updateResults.Count; i++)
        {
            var updateResult = updateResults[i];
            if (i == 0)
            {
                _updateCount = updateResult.UpdateCount;
            }
            UpdateCounts.Add(updateResult.UpdateCount);
            lastInsertId = Math.Max(lastInsertId, updateResult.LastInsertId);
        }
    }

    public void MergeUpdateCount()
    {
        _updateCount = UpdateCounts.Sum();
    }

}