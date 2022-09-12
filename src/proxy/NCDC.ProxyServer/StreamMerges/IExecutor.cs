
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.StreamMerges;

public interface IExecutor<TResult>
{
    IShardingMerger<TResult> GetShardingMerger();
    Task<List<TResult>> ExecuteAsync(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit, CancellationToken cancellationToken = new CancellationToken());
}