using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;

namespace ShardingConnector.ProxyServer.StreamMerges
{
    public sealed class DataSourceSqlExecutorUnit
    {
        public ConnectionModeEnum ConnectionMode { get; }
        public List<SqlExecutorGroup<ConnectionExecuteUnit>> SqlExecutorGroups { get; }

        public DataSourceSqlExecutorUnit(ConnectionModeEnum connectionMode,List<SqlExecutorGroup<ConnectionExecuteUnit>> sqlExecutorGroups)
        {
            ConnectionMode = connectionMode;
            SqlExecutorGroups = sqlExecutorGroups;
        }
    }
}