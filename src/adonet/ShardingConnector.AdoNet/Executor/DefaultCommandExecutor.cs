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
            var executor = new AdoNetShardingExecutor<IStreamDataReader>();
            executor.OnCommandSqlExecute += GetQueryEnumerator;
            
            return ShardingExecute<IStreamDataReader>(executor,serial, executionContext, cancellationToken);
        }

        public List<int> ExecuteNonQuery(bool serial,ExecutionContext executionContext, CancellationToken cancellationToken = new CancellationToken())
        {
            var executor = new AdoNetShardingExecutor<int>();
            executor.OnCommandSqlExecute += DoExecuteNonQuery;
            return ShardingExecute<int>(executor,serial, executionContext, cancellationToken);
        }

        private List<TResult> ShardingExecute<TResult>(IExecutor<TResult> executor,bool serial, ExecutionContext executionContext,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var waitSqlExecuteResults = executionContext.GetExecutionUnits().GroupBy(o => o.GetDataSourceName())
                .Select(o => GetSqlExecutorGroups(serial, o))
                .Select(dataSourceSqlExecutorUnit => { return executor.Execute(dataSourceSqlExecutorUnit); })
                .ToArray();
            var r = waitSqlExecuteResults.SelectMany(o=>o).ToList();
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
        private int DoExecuteNonQuery(DbCommand command, ConnectionModeEnum connectionMode)
        {
            // DbDataReader resultSet = command.ExecuteReader(sql);
            // command.CommandText = sql;
            int resultSet = command.ExecuteNonQuery();

            return resultSet;
        }

        /// <summary>
        /// 将各个数据源的数据进行分组后每组都有对应的执行单元
        /// 假如当前执行单元为x=[1,2,3,4,5,6,7,8,9]
        /// 我会首先判断当前是否是ExecuteNoQuery操作如果是serial=true
        /// 那么说明当前的所有命令需要串行执行
        /// 否则说明当前的执行命令可以进行并行,具体的并行度由_maxQueryConnectionsLimit参数值决定
        /// 如果_maxQueryConnectionsLimit=2,那么x将会被分解为y=[[1,2],[3,4],[5,6],[7,8],[9]]
        /// 我们将y的每一项称为执行组,同一个执行组里面的所有执行单元都将以并行方式执行,组与组之间将以串行方式执行
        /// 比如:循环y，第一次执行[1,2]，其中1命令和2命令是并行执行，执行完成后将执行[3,4]依次类推
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="sqlGroups"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private DataSourceSqlExecutorUnit GetSqlExecutorGroups(bool serial, IGrouping<string, ExecutionUnit> sqlGroups)
        {
            var dataSourceName = sqlGroups.Key;
            var sqlCount = sqlGroups.Count();
            

            var connectionMode = (serial|| _maxQueryConnectionsLimit >= sqlCount)
                ? ConnectionModeEnum.MEMORY_STRICTLY
                : ConnectionModeEnum.CONNECTION_STRICTLY;
            if (OnGetConnections == null)
            {
                throw new ArgumentNullException($"{nameof(OnGetConnections)}");
            }

            //如果是串行执行就是说每个组只有1个,如果是不是并行每个组有最大执行个数个
            var parallelCount = serial?1:_maxQueryConnectionsLimit;
            
            var sqlUnitPartitions = sqlGroups.Partition(parallelCount).ToArray();
            //由于分组后除了最后一个元素其余元素都满足parallelCount为最大,第一个元素的分组数将是实际的创建连接数
            var createDbConnectionCount = sqlUnitPartitions[0].Count;

            var dbConnections = OnGetConnections(connectionMode, dataSourceName, createDbConnectionCount);
            //将SqlExecutorUnit进行分区,每个区maxQueryConnectionsLimit个
            //[1,2,3,4,5,6,7],maxQueryConnectionsLimit=3,结果就是[[1,2,3],[4,5,6],[7]]
            var sqlExecutorUnitPartitions = sqlUnitPartitions
                .Select(executionUnits =>
                {
                    var commandExecuteUnits = executionUnits
                        .Select((executionUnit,i)=>CreateCommandExecuteUnit(dbConnections[i], executionUnit, connectionMode))
                        .ToList();
                    return commandExecuteUnits;
                });

            var sqlExecutorGroups = sqlExecutorUnitPartitions
                .Select(o => new SqlExecutorGroup<CommandExecuteUnit>(connectionMode, o)).ToList();
            return new DataSourceSqlExecutorUnit(connectionMode, sqlExecutorGroups);
        }

        /// <summary>
        /// 创建命令Command的执行最小单元
        /// </summary>
        /// <param name="connection">当前命令的所属链接</param>
        /// <param name="executionUnit"></param>
        /// <param name="connectionMode"></param>
        /// <returns></returns>
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