using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.StreamMerges;

public interface IShardingMerger<TResult>
{
    TResult StreamMerge(IConnectionSession connectionSession,ShardingExecutionContext shardingExecutionContext,List<TResult> parallelResults);
    void InMemoryMerge(IConnectionSession connectionSession,ShardingExecutionContext shardingExecutionContext,List<TResult> beforeInMemoryResults,List<TResult> parallelResults);
}
