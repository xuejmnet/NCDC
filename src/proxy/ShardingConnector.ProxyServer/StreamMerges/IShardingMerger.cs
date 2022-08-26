using ShardingConnector.Executor.Context;

namespace ShardingConnector.ProxyServer.StreamMerges;

public interface IShardingMerger<TResult>
{
    TResult StreamMerge(StreamMergeContext streamMergeContext,List<TResult> parallelResults);
    void InMemoryMerge(StreamMergeContext streamMergeContext,List<TResult> beforeInMemoryResults,List<TResult> parallelResults);
}
