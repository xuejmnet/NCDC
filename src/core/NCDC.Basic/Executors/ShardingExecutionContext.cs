using NCDC.Basic.Parsers;
using NCDC.CommandParser.Abstractions;

namespace NCDC.Basic.Executors
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/22 17:16:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingExecutionContext
    {
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
    
        private readonly ICollection<ExecutionUnit> _executionUnits = new HashSet<ExecutionUnit>();
        public int MaxQueryConnectionsLimit { get; }
        public ShardingExecutionContext(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            _sqlCommandContext = sqlCommandContext;
            MaxQueryConnectionsLimit = 10;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
        {
            return _sqlCommandContext;
        }

        public ICollection<ExecutionUnit> GetExecutionUnits()
        {
            return _executionUnits;
        }

        public bool IsSequenceQuery()
        {
            return false;
        }
    }
}
