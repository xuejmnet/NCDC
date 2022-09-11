using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Rewrites.Abstractions;

public interface IParameterBuilder
{
    ParameterContext GetParameterContext();
}