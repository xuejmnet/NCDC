using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.StreamMerges;

public interface IShardingMerger<TResult>
{
    TResult StreamMerge(ShardingExecutionContext shardingExecutionContext,List<TResult> parallelResults);
    void InMemoryMerge(ShardingExecutionContext shardingExecutionContext,List<TResult> beforeInMemoryResults,List<TResult> parallelResults);
}
