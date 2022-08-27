using ShardingConnector.Executor.Context;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.ProxyServer.Options.Context;

namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class StreamDataReaderShardingMerger:IShardingMerger<IStreamDataReader>
{
    public static StreamDataReaderShardingMerger Instance { get; } = new StreamDataReaderShardingMerger();
    public IStreamDataReader StreamMerge(ShardingExecutionContext shardingExecutionContext,List<IStreamDataReader> streamDataReaders)
    {
        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
        MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(),
            runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
        return mergeEngine.Merge(streamDataReaders.ToList(), shardingExecutionContext.GetSqlCommandContext());
    }

    public void InMemoryMerge(ShardingExecutionContext shardingExecutionContext,List<IStreamDataReader> beforeInMemoryResults, List<IStreamDataReader> parallelResults)
    {
       
    }
}