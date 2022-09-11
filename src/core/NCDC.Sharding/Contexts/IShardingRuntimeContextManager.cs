namespace NCDC.Sharding.Contexts;

public interface IShardingRuntimeContextManager
{
    IShardingRuntimeContext GetShardingRuntimeContext(string logicDatabaseName);
}