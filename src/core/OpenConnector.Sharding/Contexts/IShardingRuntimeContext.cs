using OpenConnector.CommandParser.Abstractions;
using OpenConnector.Sharding.Configurations;
using OpenConnector.Sharding.ShardingComparisions;

namespace OpenConnector.Sharding.Contexts;

public interface IShardingRuntimeContext
{
    string LogicDatabaseName { get; }
    ShardingConfiguration GetShardingConfiguration();
    IShardingComparer GetShardingComparer();
    
}