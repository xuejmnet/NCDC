using OpenConnector.Configuration;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.StreamMerges.Executors.Context
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