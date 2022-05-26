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
        List<DbConnection> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize);
    
        CommandExecuteUnit CreateCommandExecuteUnit(DbConnection connection, ExecutionUnit executionUnit, ConnectionModeEnum connectionMode);

    }
}