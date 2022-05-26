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
        public List<IStreamDataReader> Execute(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
        {
            return Execute0(dataSourceSqlExecutorUnit);
        }

        private List<IStreamDataReader> Execute0(DataSourceSqlExecutorUnit dataSourceSqlExecutorUnit)
        {
            var executorGroups = dataSourceSqlExecutorUnit.SqlExecutorGroups;
            var result = new List<IStreamDataReader>(executorGroups.Sum(o=>o.Groups.Count()));
            foreach (var executorGroup in executorGroups)
            {
                var routeQueryResults =  GroupExecute(executorGroup.Groups);
                result.AddAll(routeQueryResults);
            }

            return result;
        }

        private  IStreamDataReader[] GroupExecute(List<CommandExecuteUnit> commandExecuteUnits)
        {
            if (commandExecuteUnits.Count <= 0)
            {
                return Array.Empty<IStreamDataReader>();
            }

            CancellationToken cancellationToken = new CancellationToken();
            var dataReaders = new List<IStreamDataReader>(commandExecuteUnits.Count());
            var otherTasks = commandExecuteUnits.Skip(1)
                .Select(o => Task.Run(() => ExecuteCommandUnit(o), cancellationToken)).ToArray();
            var streamDataReader = ExecuteCommandUnit(commandExecuteUnits[0]);
            var streamDataReaders = Task.WhenAll(otherTasks).GetAwaiter().GetResult();
            dataReaders.Add(streamDataReader);
            dataReaders.AddAll(streamDataReaders);
            return dataReaders.ToArray();
        }

        private  IStreamDataReader ExecuteCommandUnit(CommandExecuteUnit commandExecuteUnit)
        {
            if (OnCommandSqlExecute == null)
            {
                throw new ShardingException("command sql execute not implement");
            }
            return OnCommandSqlExecute?.Invoke(commandExecuteUnit.Command, commandExecuteUnit.ConnectionMode);
        }
    }
}