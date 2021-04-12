using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Contexts
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/22 17:16:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ExecutionContext
    {
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
    
        private readonly ICollection<ExecutionUnit> _executionUnits = new HashSet<ExecutionUnit>();

        public ExecutionContext(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            _sqlCommandContext = sqlCommandContext;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlStatementContext()
        {
            return _sqlCommandContext;
        }

        public ICollection<ExecutionUnit> GetExecutionUnits()
        {
            return _executionUnits;
        }
    }
}
