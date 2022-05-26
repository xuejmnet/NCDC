using System.Collections.Generic;
using ShardingConnector.Executor.Constant;

namespace ShardingConnector.Executor.Context
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