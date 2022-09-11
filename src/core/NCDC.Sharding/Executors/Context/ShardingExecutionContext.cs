using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;

namespace NCDC.Sharding.Executors.Context
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
        public bool IsSerialExecute { get; }
        public bool IsSelect => !IsSerialExecute;

        public ShardingExecutionContext(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            _sqlCommandContext = sqlCommandContext;
            MaxQueryConnectionsLimit = 10;
            IsSerialExecute = !(sqlCommandContext is SelectCommandContext);
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
