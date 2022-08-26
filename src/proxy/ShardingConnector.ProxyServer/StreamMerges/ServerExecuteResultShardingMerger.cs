using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.StreamMerges.Results;

namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class ServerExecuteResultShardingMerger:IShardingMerger<IExecuteResult>
{
    private ServerExecuteResultShardingMerger(){}
    public static ServerExecuteResultShardingMerger Instance = new ServerExecuteResultShardingMerger();
    public IExecuteResult StreamMerge(StreamMergeContext streamMergeContext, List<IExecuteResult> parallelResults)
    {
        var parallelResult = parallelResults[0];
        if (parallelResult is QueryExecuteResult queryExecuteResult)
        {
            var dbColumns = queryExecuteResult.DbColumns;
            ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
            MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(),
                runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
            var streamDataReader = mergeEngine.Merge(parallelResults.Select(o=>((QueryExecuteResult)o).StreamDataReader).ToList(), streamMergeContext.GetSqlCommandContext());
            return new QueryExecuteResult(dbColumns, streamDataReader);
        }
        else
        {
            int recordsAffected = 0;
            long lastInsertId=0L;
            foreach (var r in parallelResults)
            {
                var affectRowExecuteResult = (AffectRowExecuteResult)r;
                recordsAffected += affectRowExecuteResult.RecordsAffected;
                lastInsertId = Math.Max(lastInsertId, affectRowExecuteResult.LastInsertId);
            }

            return new AffectRowExecuteResult(recordsAffected, lastInsertId);
        }
    }

    public void InMemoryMerge(StreamMergeContext streamMergeContext, List<IExecuteResult> beforeInMemoryResults, List<IExecuteResult> parallelResults)
    {
        throw new NotImplementedException();
    }
}