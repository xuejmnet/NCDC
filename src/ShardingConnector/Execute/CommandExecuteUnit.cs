using System.Data.Common;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Execute
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/15 16:38:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommandExecuteUnit
    {
        public ExecutionUnit ExecutionUnit { get; }
    
        public DbCommand Command{ get; }
    
        public ConnectionModeEnum ConnectionMode{ get; }

        public CommandExecuteUnit(ExecutionUnit executionUnit, DbCommand command, ConnectionModeEnum connectionMode)
        {
            ExecutionUnit = executionUnit;
            Command = command;
            ConnectionMode = connectionMode;
        }
    }
}