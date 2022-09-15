using NCDC.CommandParser.Abstractions;

namespace NCDC.ProxyServer.Executors;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(string sql);
    ShardingExecutionContext Create(string sql,ISqlCommand sqlCommand);
}