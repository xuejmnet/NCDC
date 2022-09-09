using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Rewrites.Abstractions;

public interface IParameterBuilder
{
    ParameterContext GetParameterContext();
}