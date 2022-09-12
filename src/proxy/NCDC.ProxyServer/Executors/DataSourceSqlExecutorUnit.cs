using NCDC.Enums;

namespace NCDC.ProxyServer.Executors
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