using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.Executor;
using ShardingConnector.Executor.Constant;
using ShardingConnector.ShardingExecute.Execute;
using ShardingConnector.ShardingExecute.Execute.DataReader;
using System;
using System.Collections.Generic;
using System.Data.Common;

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
        public CommandExecutor(int resultSetType, int resultSetConcurrency, int resultSetHoldability, ShardingConnection shardingConnection) : base(resultSetType, resultSetConcurrency, resultSetHoldability, shardingConnection)
        {
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
            SqlExecuteCallback<IQueryEnumerator> executeCallback = new SqlExecuteCallback<IQueryEnumerator>(databaseType, isExceptionThrown);
            executeCallback.OnSqlExecute += GetQueryEnumerator;
            return ExecuteCallback(executeCallback);
        }

        private IQueryEnumerator GetQueryEnumerator(String sql, DbCommand statement, ConnectionModeEnum connectionMode)
        {
            // DbDataReader resultSet = statement.ExecuteReader(sql);
            DbDataReader resultSet = statement.ExecuteReader();
            resultSets.Add(resultSet);
            if (ConnectionModeEnum.MEMORY_STRICTLY != connectionMode)
                return new StreamQueryDataReader(resultSet);
            return new MemoryQueryDataReader(resultSet);
        }
    }
}