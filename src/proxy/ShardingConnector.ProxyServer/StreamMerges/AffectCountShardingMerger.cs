using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.Executor.Context;
using ShardingConnector.Pluggable.Merge;

namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class AffectCountShardingMerger:IShardingMerger<int>
{
    public static AffectCountShardingMerger Instance { get; } = new AffectCountShardingMerger();
    public int StreamMerge(StreamMergeContext streamMergeContext,List<int> streamDataReaders)
    {
        return streamDataReaders.Sum();
    }

    public void InMemoryMerge(StreamMergeContext streamMergeContext,List<int> beforeInMemoryResults, List<int> parallelResults)
    {
        beforeInMemoryResults.AddRange(parallelResults);
    }
}