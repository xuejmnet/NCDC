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
            var dbConnections = _commandExecutor.Connection.GetConnections(connectionMode, dataSourceName, connectionSize);
            dbConnections.ForEach(o=>o.Open());
            return dbConnections;
        }

        public CommandExecuteUnit CreateStatementExecuteUnit(DbConnection connection, ExecutionUnit executionUnit,
            ConnectionModeEnum connectionMode)
        {
            return new CommandExecuteUnit(executionUnit, connection.CreateCommand(), connectionMode);
        }

    }
}
