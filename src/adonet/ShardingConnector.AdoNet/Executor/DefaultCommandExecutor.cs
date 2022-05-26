using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.Helpers;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ShardingExecute.Execute.DataReader;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.AdoNet.Executor
{
    public class DefaultCommandExecutor:ICommandExecutor
    {
        private readonly ShardingConnection _shardingConnection;
        private readonly int _maxQueryConnectionsLimit;
        private readonly List<DbDataReader> _dbDataReaders = new List<DbDataReader>();

        public DefaultCommandExecutor(ShardingConnection shardingConnection,int maxQueryConnectionsLimit)
        {
            _shardingConnection = shardingConnection;
            _maxQueryConnectionsLimit = maxQueryConnectionsLimit;
        }
        public List<IStreamDataReader> ExecuteDbDataReader(bool serial, ExecutionContext executionContext,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var dataReaderExecutor = new DataReaderExecutor();
            dataReaderExecutor.OnCommandSqlExecute += GetQueryEnumerator;
            var waitSqlExecuteResults = executionContext.GetExecutionUnits().GroupBy(o=>o.GetDataSourceName())
                .Select(o=>GetSqlExecutorGroups(serial,o))
                .Select(dataSourceSqlExecutorUnit =>
                {
                    return dataReaderExecutor.Execute(dataSourceSqlExecutorUnit);
                }).ToArray();
            var r = waitSqlExecuteResults.SelectMany(o=>o).ToList();

            return r;
        }

        public List<DbDataReader> GetDataReaders()
        {
            return _dbDataReaders;
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

        private DataSourceSqlExecutorUnit GetSqlExecutorGroups(bool serial,IGrouping<string, ExecutionUnit> sqlGroups)
        {
            var dataSourceName = sqlGroups.Key;
            var sqlCount = sqlGroups.Count();
            var connectionMode = (!serial&&_maxQueryConnectionsLimit<=sqlCount)?ConnectionModeEnum.CONNECTION_STRICTLY:ConnectionModeEnum.MEMORY_STRICTLY;
            var dbConnections = _shardingConnection.GetConnections(connectionMode,dataSourceName,sqlCount);
            //将SqlExecutorUnit进行分区,每个区maxQueryConnectionsLimit个
            //[1,2,3,4,5,6,7],maxQueryConnectionsLimit=3,结果就是[[1,2,3],[4,5,6],[7]]
            var sqlExecutorUnitPartitions = sqlGroups.Select((o,i)=>CreateCommandExecuteUnit(dbConnections[i],o,connectionMode)).Partition(_maxQueryConnectionsLimit);
            
            var sqlExecutorGroups = sqlExecutorUnitPartitions.Select(o => new SqlExecutorGroup<CommandExecuteUnit>(connectionMode, o)).ToList();
            return new DataSourceSqlExecutorUnit(connectionMode, sqlExecutorGroups);
        }
        private CommandExecuteUnit CreateCommandExecuteUnit(DbConnection connection, ExecutionUnit executionUnit,
            ConnectionModeEnum connectionMode)
        {
            var shardingParameters = executionUnit.GetSqlUnit().GetParameterContext().GetDbParameters().Select(o=>(ShardingParameter)o).ToList();
            var dbCommand = connection.CreateCommand();
            //TODO取消手动执行改成replay
            dbCommand.CommandText = executionUnit.GetSqlUnit().GetSql();
            foreach (var shardingParameter in shardingParameters)
            {
                var dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = shardingParameter.ParameterName;
                dbParameter.Value = shardingParameter.Value;
                dbCommand.Parameters.Add(dbParameter);
            }
            return new CommandExecuteUnit(executionUnit,dbCommand , connectionMode);
        }

    }
}