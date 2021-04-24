using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.ShardingExecute.Execute;
using ShardingConnector.ShardingExecute.Prepare;

namespace ShardingConnector.AdoNet.Executor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 14:35:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class CommandGetGroupsSqlExecutePrepareCallback: ISqlExecutePrepareCallback
    {
        private readonly CommandExecutor _commandExecutor;

        public CommandGetGroupsSqlExecutePrepareCallback(CommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }
        public List<DbConnection> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
        {
            return _commandExecutor.Connection.c(connectionMode, dataSourceName, connectionSize);
        }

        public CommandExecuteUnit CreateStatementExecuteUnit(DbConnection connection, ExecutionUnit executionUnit,
            ConnectionModeEnum connectionMode)
        {
            throw new NotImplementedException();
        }


        @Override
        public List<Connection> getConnections(final ConnectionMode connectionMode, final String dataSourceName, final int connectionSize) throws SQLException
        {
    }

    @SuppressWarnings("MagicConstant")
    @Override
    public StatementExecuteUnit createStatementExecuteUnit(final Connection connection, final ExecutionUnit executionUnit, final ConnectionMode connectionMode) throws SQLException
    {
    return new StatementExecuteUnit(executionUnit, connection.createStatement(getResultSetType(), getResultSetConcurrency(), getResultSetHoldability()), connectionMode);
}
    }
}
