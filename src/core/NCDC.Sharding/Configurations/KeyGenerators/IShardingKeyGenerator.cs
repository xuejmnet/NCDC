namespace NCDC.Sharding.Configurations.KeyGenerators;

public interface IShardingKeyGenerator
{
    IComparable GenerateKey();
}