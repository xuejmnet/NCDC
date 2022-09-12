using NCDC.ProxyServer.StreamMerges.Executors.Context;

namespace NCDC.ProxyServer.StreamMerges;

public interface IExecutor<TResult>
{
    IShardingMerger<TResult> GetShardingMerger();
    Task<List<TResult>> ExecuteAsync(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit, CancellationToken cancellationToken = new CancellationToken());
}