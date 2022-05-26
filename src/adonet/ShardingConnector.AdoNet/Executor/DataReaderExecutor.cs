using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.Helpers;
using ShardingConnector.ParserBinder.Command;

namespace ShardingConnector.AdoNet.Executor
{
    public class DataReaderExecutor:IExecutor<IStreamDataReader>
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
        public event Func<DbCommand, ConnectionModeEnum, IStreamDataReader> OnCommandSqlExecute;

        public DataReaderExecutor()
        {
            // ISqlCommandContext<ISqlCommand> sqlCommandContext
            // _sqlCommandContext = sqlCommandContext;
        }
        // protected void Cancel()
        // {
        //     Interlocked.Exchange(ref cancelStatus, cancelled);
        // }
        //
        // private bool IsCancelled()
        // {
        //     return cancelStatus == cancelled;
        // }
        public Task<List<IStreamDataReader>> ExecuteAsync(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return ExecuteAsync0(dataSourceSqlExecutorUnit, cancellationToken);
        }

        private async Task<List<IStreamDataReader>> ExecuteAsync0(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var executorGroups = dataSourceSqlExecutorUnit.SqlExecutorGroups;
            var result = new List<IStreamDataReader>(executorGroups.Sum(o=>o.Groups.Count()));
            foreach (var executorGroup in executorGroups)
            {
                var routeQueryResults = await GroupExecuteAsync(executorGroup.Groups,cancellationToken);
                result.AddAll(routeQueryResults);
            }

            return result;
        }

        private  Task<IStreamDataReader[]> GroupExecuteAsync(List<CommandExecuteUnit> commandExecuteUnits,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (commandExecuteUnits.Count <= 0)
            {
                return Task.FromResult(Array.Empty<IStreamDataReader>());
            }

            var tasks = commandExecuteUnits.Select(o=>ExecuteCommandUnitAsync(o,cancellationToken)).ToArray();
            return TaskHelper.WhenAllFastFail(tasks);
        }

        private  Task<IStreamDataReader> ExecuteCommandUnitAsync(CommandExecuteUnit commandExecuteUnit,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (OnCommandSqlExecute == null)
            {
                throw new ShardingException("command sql execute not implement");
            }
            return Task.Run(()=>OnCommandSqlExecute?.Invoke(commandExecuteUnit.Command, commandExecuteUnit.ConnectionMode), cancellationToken);
        }
    }
}