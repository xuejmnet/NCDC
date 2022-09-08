namespace OpenConnector.Sharding.Configurations.KeyGenerators;

public class GuidShardingKeyGenerator:IShardingKeyGenerator
{
    public IComparable GenerateKey()
    {
        return Guid.NewGuid();
    }
}