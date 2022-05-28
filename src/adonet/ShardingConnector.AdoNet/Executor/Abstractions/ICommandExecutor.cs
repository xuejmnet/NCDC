using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.AdoNet.Executor.Abstractions
{
    public interface ICommandExecutor
    {
        List<IStreamDataReader> ExecuteDbDataReader(bool serial, ExecutionContext executionContext, CancellationToken cancellationToken=new CancellationToken());
        List<DbDataReader> GetDataReaders();
        void Clear();
    }
}