using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;

namespace ShardingConnector.ProxyServer.StreamMerges
{
    public sealed class DataSourceSqlExecutorUnit
    {
        public ConnectionModeEnum ConnectionMode { get; }
        public List<SqlExecutorGroup<CommandExecuteUnit>> SqlExecutorGroups { get; }

        public DataSourceSqlExecutorUnit(ConnectionModeEnum connectionMode,List<SqlExecutorGroup<CommandExecuteUnit>> sqlExecutorGroups)
        {
            ConnectionMode = connectionMode;
            SqlExecutorGroups = sqlExecutorGroups;
        }
    }
}