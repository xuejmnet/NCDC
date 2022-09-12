using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.ServerHandlers.Results;

public class RecordsAffectedServerResult:IServerResult
{
    public static RecordsAffectedServerResult Empty { get; } = new RecordsAffectedServerResult();
    public ResultTypeEnum ResultType => ResultTypeEnum.UPDATE;
    public long LastInsertId { get; }
    public List<int> UpdateCounts { get; }
    public long UpdateCount { get; private set; }
    public RecordsAffectedServerResult():this(new List<AffectRowUnitResult>(0))
    {
        
    }

    public RecordsAffectedServerResult(List<AffectRowUnitResult> updateResults)
    {
        
        UpdateCounts = new List<int>(updateResults.Count);
        long lastInsertId = 0;
        for (var i = 0; i < updateResults.Count; i++)
        {
            var updateResult = updateResults[i];
            if (i == 0)
            {
                UpdateCount = updateResult.UpdateCount;
            }
            UpdateCounts.Add(updateResult.UpdateCount);
            lastInsertId = Math.Max(lastInsertId, updateResult.LastInsertId);
        }
    }
    public void MergeUpdateCount()
    {
        UpdateCount = UpdateCounts.Sum();
    }
}