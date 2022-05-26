using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.Executor.Context;

namespace ShardingConnector.AdoNet.Executor.Abstractions
{
    public interface IExecutor<TResult>
    {
        Task<List<TResult>> ExecuteAsync(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit,
            CancellationToken cancellationToken = new CancellationToken());
    }
}