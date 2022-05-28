using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingExecute.Execute.DataReader;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.AdoNet.Executor
{
    public class DefaultCommandExecutor : ICommandExecutor
    {
        private readonly int _maxQueryConnectionsLimit;
        private readonly ConcurrentBag<DbDataReader> _dbDataReaders = new ConcurrentBag<DbDataReader>();

        public event Func<ConnectionModeEnum, string /*dataSourceName*/, int /*connectionSize*/, List<DbConnection>>
            OnGetConnections;

        public DefaultCommandExecutor(int maxQueryConnectionsLimit)
        {
            _maxQueryConnectionsLimit = maxQueryConnectionsLimit;
        }

        public List<IStreamDataReader> ExecuteDbDataReader(bool serial, ExecutionContext executionContext,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var dataReaderExecutor = new DataReaderExecutor();
            dataReaderExecutor.OnCommandSqlExecute += GetQueryEnumerator;
            var waitSqlExecuteResults = executionContext.GetExecutionUnits().GroupBy(o => o.GetDataSourceName())
                .Select(o => GetSqlExecutorGroups(serial, o))
                .Select(dataSourceSqlExecutorUnit => { return dataReaderExecutor.Execute(dataSourceSqlExecutorUnit); })
                .ToArray();
            var r = waitSqlExecuteResults.SelectMany(o => o).ToList();

            return r;
        }

        public List<DbDataReader> GetDataReaders()
        {
            return _dbDataReaders.ToList();
        }

        public void Clear()
        {
            foreach (var dbDataReader in GetDataReaders())
            {
                dbDataReader.Dispose();
            }
            GetDataReaders().Clear();
        }

        private IStreamDataReader GetQueryEnumerator(DbCommand command, ConnectionModeEnum connectionMode)
        {
            // DbDataReader resultSet = command.ExecuteReader(sql);
            // command.CommandText = sql;
            DbDataReader resultSet = command.ExecuteReader();
            _dbDataReaders.Add(resultSet);
            if (ConnectionModeEnum.MEMORY_STRICTLY == connectionMode)
                return new StreamQueryDataReader(resultSet);
            return new MemoryQueryDataReader(resultSet);
        }

        private DataSourceSqlExecutorUnit GetSqlExecutorGroups(bool serial, IGrouping<string, ExecutionUnit> sqlGroups)
        {
            var dataSourceName = sqlGroups.Key;
            var sqlCount = sqlGroups.Count();
            var connectionMode = (!serial && _maxQueryConnectionsLimit <= sqlCount)
                ? ConnectionModeEnum.CONNECTION_STRICTLY
                : ConnectionModeEnum.MEMORY_STRICTLY;
            if (OnGetConnections == null)
            {
                throw new ArgumentNullException($"{nameof(OnGetConnections)}");
            }

            var dbConnections = OnGetConnections(connectionMode, dataSourceName, sqlCount);
            //将SqlExecutorUnit进行分区,每个区maxQueryConnectionsLimit个
            //[1,2,3,4,5,6,7],maxQueryConnectionsLimit=3,结果就是[[1,2,3],[4,5,6],[7]]
            var sqlExecutorUnitPartitions = sqlGroups
                .Select((o, i) => CreateCommandExecuteUnit(dbConnections[i], o, connectionMode))
                .Partition(_maxQueryConnectionsLimit);

            var sqlExecutorGroups = sqlExecutorUnitPartitions
                .Select(o => new SqlExecutorGroup<CommandExecuteUnit>(connectionMode, o)).ToList();
            return new DataSourceSqlExecutorUnit(connectionMode, sqlExecutorGroups);
        }

        private CommandExecuteUnit CreateCommandExecuteUnit(DbConnection connection, ExecutionUnit executionUnit,
            ConnectionModeEnum connectionMode)
        {
            var commandText = executionUnit.GetSqlUnit().GetSql();
            
            var shardingParameters = executionUnit.GetSqlUnit().GetParameterContext().GetDbParameters()
                .Select(o => (ShardingParameter)o).ToList();
            var dbCommand = connection.CreateCommand();
            //TODO取消手动执行改成replay
            dbCommand.CommandText = commandText;
            foreach (var shardingParameter in shardingParameters)
            {
                var dbParameter = dbCommand.CreateParameter();
                shardingParameter.ReplyTargetMethodInvoke(dbParameter);
                dbCommand.Parameters.Add(dbParameter);
            }

            return new CommandExecuteUnit(executionUnit, dbCommand, connectionMode);
        }
    }
}