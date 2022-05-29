using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingExecute.Execute;

namespace ShardingConnector.AdoNet.Executor
{
    public class AdoNetShardingExecutor<TResult>:IExecutor<TResult>
    {
        // /// <summary>
        // /// not cancelled const mark
        // /// </summary>
        // private const int notCancelled = 1;
        //
        // /// <summary>
        // /// cancelled const mark
        // /// </summary>
        // private const int cancelled = 0;
        // /// <summary>
        // /// cancel status
        // /// </summary>
        // private int cancelStatus= notCancelled;
        // private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
        public event Func<DbCommand, ConnectionModeEnum, TResult> OnCommandSqlExecute;

      
        // protected void Cancel()
        // {
        //     Interlocked.Exchange(ref cancelStatus, cancelled);
        // }
        //
        // private bool IsCancelled()
        // {
        //     return cancelStatus == cancelled;
        // }
        public List<TResult> Execute(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
        {
            return Execute0(dataSourceSqlExecutorUnit);
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

        private  TResult[] GroupExecute(List<CommandExecuteUnit> commandExecuteUnits)
        {
            if (commandExecuteUnits.Count <= 0)
            {
                return Array.Empty<TResult>();
            }

            if (commandExecuteUnits.Count == 1)
            {
                return new TResult[1] { ExecuteCommandUnit(commandExecuteUnits[0]) };
            }
            else
            { 
                CancellationToken cancellationToken = new CancellationToken();
                var dataReaders = new List<TResult>(commandExecuteUnits.Count());
                var otherTasks = commandExecuteUnits.Skip(1)
                    .Select(o => Task.Run(() => ExecuteCommandUnit(o), cancellationToken)).ToArray();
                var streamDataReader = ExecuteCommandUnit(commandExecuteUnits[0]);
                var streamDataReaders = Task.WhenAll(otherTasks).GetAwaiter().GetResult();
                dataReaders.Add(streamDataReader);
                dataReaders.AddAll(streamDataReaders);
                return dataReaders.ToArray();
            }
        }

        private  TResult ExecuteCommandUnit(CommandExecuteUnit commandExecuteUnit)
        {
            if (OnCommandSqlExecute == null)
            {
                throw new ShardingException("command sql execute not implement");
            }
            return OnCommandSqlExecute.Invoke(commandExecuteUnit.Command, commandExecuteUnit.ConnectionMode);
        }
    }
}