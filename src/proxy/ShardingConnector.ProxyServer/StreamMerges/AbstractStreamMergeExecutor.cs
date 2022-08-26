using System.Data.Common;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ShardingExecute.Execute.DataReader;

namespace ShardingConnector.ProxyServer.StreamMerges;

public abstract class AbstractStreamMergeExecutor<TResult>:IExecutor<TResult>
{
    public abstract IShardingMerger<TResult> GetShardingMerger();

    public Task<List<TResult>> ExecuteAsync(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var tResults = Execute0(dataSourceSqlExecutorUnit);
        return Task.FromResult(tResults);
    }
    
    
        private List<TResult> Execute0(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
        {
            var executorGroups = dataSourceSqlExecutorUnit.SqlExecutorGroups;
            var result = new List<TResult>(executorGroups.Sum(o=>o.Groups.Count()));
            foreach (var executorGroup in executorGroups)
            {
                var routeQueryResults =  GroupExecute(executorGroup.Groups);
                result.AddAll(routeQueryResults);
            }

            return result;
        }

        private  TResult[] GroupExecute(List<ConnectionExecuteUnit> connectionExecuteUnits)
        {
            if (connectionExecuteUnits.Count <= 0)
            {
                return Array.Empty<TResult>();
            }

            if (connectionExecuteUnits.Count == 1)
            {
                return new TResult[1] { ExecuteConnectionUnit(connectionExecuteUnits[0]) };
            }
            else
            { 
                CancellationToken cancellationToken = new CancellationToken();
                var dataReaders = new List<TResult>(connectionExecuteUnits.Count());
                var otherTasks = connectionExecuteUnits.Skip(1)
                    .Select(o => Task.Run(() => ExecuteConnectionUnit(o), cancellationToken)).ToArray();
                var streamDataReader = ExecuteConnectionUnit(connectionExecuteUnits[0]);
                var streamDataReaders = Task.WhenAll(otherTasks).GetAwaiter().GetResult();
                dataReaders.Add(streamDataReader);
                dataReaders.AddAll(streamDataReaders);
                return dataReaders.ToArray();
            }
        }

        protected abstract  TResult ExecuteConnectionUnit(ConnectionExecuteUnit commandExecuteUnit);
        // {
        //     // DbDataReader resultSet = command.ExecuteReader(sql);
        //     // command.CommandText = sql;
        //     DbDataReader resultSet = command.ExecuteReader();
        //     if (ConnectionModeEnum.MEMORY_STRICTLY == connectionMode)
        //         return new StreamQueryDataReader(resultSet);
        //     return new MemoryQueryDataReader(resultSet);
        // }
}