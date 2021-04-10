using System.Collections.Generic;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Kernels.Parse
{
/*
* @Author: xjm
* @Description:1
* @Date: Tuesday, 23 March 2021 21:27:41
* @Email: 326308290@qq.com
*/
    public class ExecutionContext<T> where T:ISqlCommand
    {
        private readonly ISqlCommandContext<T> _sqlCommandContext;
        private readonly ICollection<ExecutionUnit> _executionUnits = new LinkedList<ExecutionUnit>();

        public ExecutionContext(ISqlCommandContext<T> sqlCommandContext)
        {
            _sqlCommandContext = sqlCommandContext;
        }
    }
}