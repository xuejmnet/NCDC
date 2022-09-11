using NCDC.ShardingAdoNet;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IParameterBuilder
{
    ParameterContext GetParameterContext();
}