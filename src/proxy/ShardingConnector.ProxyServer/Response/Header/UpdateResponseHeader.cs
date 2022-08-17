using System.Collections.Immutable;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Extensions;

namespace ShardingConnector.ProxyServer.Response.Header;

public sealed class UpdateResponseHeader:IResponseHeader
{
    public long LastInsertId { get; }
    public List<int> UpdateCounts { get; }
    private long _updateCount;

    public UpdateResponseHeader():this(new List<UpdateResult>(0))
    {
        
    }

    public long GetUpdateCount()
    {
        return _updateCount;
    }

    public UpdateResponseHeader(List<UpdateResult> updateResults)
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