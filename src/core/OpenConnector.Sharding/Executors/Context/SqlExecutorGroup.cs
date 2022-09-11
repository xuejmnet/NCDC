using OpenConnector.Configuration;

namespace OpenConnector.Sharding.Executors.Context
{
    public sealed class SqlExecutorGroup<T>
    {
        public ConnectionModeEnum ConnectionMode { get; }
        public List<T> Groups { get; }

        public SqlExecutorGroup(ConnectionModeEnum connectionMode,List<T> groups)
        {
            ConnectionMode = connectionMode;
            Groups = groups;
        }
    }
}