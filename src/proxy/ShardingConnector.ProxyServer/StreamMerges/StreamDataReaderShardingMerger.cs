using ShardingConnector.Executor.Context;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.ProxyServer.Options.Context;

namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class StreamDataReaderShardingMerger:IShardingMerger<IStreamDataReader>
{
    public static StreamDataReaderShardingMerger Instance { get; } = new StreamDataReaderShardingMerger();
    public IStreamDataReader StreamMerge(StreamMergeContext streamMergeContext,List<IStreamDataReader> streamDataReaders)
    {
        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
        MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(),
            runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
        return mergeEngine.Merge(streamDataReaders.ToList(), streamMergeContext.GetSqlCommandContext());
    }

    public void InMemoryMerge(StreamMergeContext streamMergeContext,List<IStreamDataReader> beforeInMemoryResults, List<IStreamDataReader> parallelResults)
    {
       
    }
}