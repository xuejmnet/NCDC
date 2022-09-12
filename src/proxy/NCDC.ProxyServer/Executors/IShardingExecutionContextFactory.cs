namespace NCDC.ProxyServer.Executors;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(string sql);
}