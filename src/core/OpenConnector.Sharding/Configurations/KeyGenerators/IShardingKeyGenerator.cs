namespace OpenConnector.Sharding.Configurations.KeyGenerators;

public interface IShardingKeyGenerator
{
    IComparable GenerateKey();
}