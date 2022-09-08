namespace OpenConnector.Sharding.Contexts;

public interface IShardingRuntimeContextManager
{
    IShardingRuntimeContext GetShardingRuntimeContext(string logicDatabaseName);
}