using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.Executor;
using ShardingConnector.Executor.Constant;
using ShardingConnector.ShardingExecute.Execute;
using ShardingConnector.ShardingExecute.Execute.DataReader;
using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Executor.Context;
using ShardingConnector.Executor.Engine;
using ShardingConnector.Extensions;

namespace ShardingConnector.AdoNet.Executor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Friday, 16 April 2021 21:47:47
    * @Email: 326308290@qq.com
    */
    public class CommandExecutor : AbstractCommandExecutor
    {
        public CommandExecutor(ShardingConnection shardingConnection) : base(shardingConnection)
        {
        }


        public void init(ExecutionContext executionContext)
        {
            SqlStatementContext = executionContext.GetSqlStatementContext();
            InputGroups.AddAll();
        getInputGroups().addAll(getExecuteGroups(executionContext.getExecutionUnits()));
        cacheStatements();
        }

        private ICollection<InputGroup<CommandExecuteUnit>> getExecuteGroups(ICollection<ExecutionUnit> executionUnits)
        {
            SqlExecutePrepareTemplate.GetExecuteUnitGroups(executionUnits,)
            return getSqlExecutePrepareTemplate().getExecuteUnitGroups(executionUnits, new SQLExecutePrepareCallback()
        {

            @Override
            public List<Connection> getConnections(final ConnectionMode connectionMode, final String dataSourceName, final int connectionSize) throws SQLException {
                return StatementExecutor.super.getConnection().getConnections(connectionMode, dataSourceName, connectionSize);
            }

            @SuppressWarnings("MagicConstant")
            @Override
            public StatementExecuteUnit createStatementExecuteUnit(final Connection connection, final ExecutionUnit executionUnit, final ConnectionMode connectionMode) throws SQLException {
                return new StatementExecuteUnit(executionUnit, connection.createStatement(getResultSetType(), getResultSetConcurrency(), getResultSetHoldability()), connectionMode);
            }
        });
    }
    /**
 * Execute query.
 * 
 * @return result set list
 * @throws SQLException SQL exception
 */
    public List<IQueryEnumerator> ExecuteQuery()
        {
            var isExceptionThrown = ExecutorExceptionHandler.IsThrowException();
            SqlExecuteCallback<IQueryEnumerator> executeCallback = new SqlExecuteCallback<IQueryEnumerator>(DatabaseType, isExceptionThrown);
            executeCallback.OnSqlExecute += GetQueryEnumerator;
            return ExecuteCallback(executeCallback);
        }

        private IQueryEnumerator GetQueryEnumerator(String sql, DbCommand statement, ConnectionModeEnum connectionMode)
        {
            // DbDataReader resultSet = statement.ExecuteReader(sql);
            DbDataReader resultSet = statement.ExecuteReader();
            ResultSets.Add(resultSet);
            if (ConnectionModeEnum.MEMORY_STRICTLY != connectionMode)
                return new StreamQueryDataReader(resultSet);
            return new MemoryQueryDataReader(resultSet);
        }
    }
}