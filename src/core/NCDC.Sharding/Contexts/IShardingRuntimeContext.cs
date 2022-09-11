using NCDC.CommandParser.Abstractions;
using NCDC.Sharding.Configurations;
using NCDC.Sharding.ShardingComparisions;

namespace NCDC.Sharding.Contexts;

public interface IShardingRuntimeContext
{
    string LogicDatabaseName { get; }
    ShardingConfiguration GetShardingConfiguration();
    IShardingComparer GetShardingComparer();
    
}