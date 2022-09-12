namespace NCDC.Basic.Executors;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(string sql);
}