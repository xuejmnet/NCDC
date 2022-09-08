namespace OpenConnector.Sharding.Configurations.KeyGenerators;

public class NotSupportShardingKeyGenerator:IShardingKeyGenerator
{
    public IComparable GenerateKey()
    {
        throw new NotImplementedException();
    }
}