using ShardingConnector.Executor.Constant;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Executor.Context;
using ShardingConnector.ShardingExecute.Execute;

namespace ShardingConnector.ShardingExecute.Prepare
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface ISqlExecutePrepareCallback
    {
        /**
     * Get connection.
     * 
     * @param connectionMode connection mode
     * @param dataSourceName data source name
     * @param connectionSize connection size
     * @return connection
     * @throws SQLException SQL exception
     */
        List<DbConnection> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize);
    
        /**
     * Create SQL execute unit.
     * 
     * @param connection connection
     * @param executionUnit execution unit
     * @param connectionMode connection mode
     * @return SQL execute unit
     * @throws SQLException SQL exception
     */
        CommandExecuteUnit CreateCommandExecuteUnit(DbConnection connection, ExecutionUnit executionUnit, ConnectionModeEnum connectionMode);

    }
}