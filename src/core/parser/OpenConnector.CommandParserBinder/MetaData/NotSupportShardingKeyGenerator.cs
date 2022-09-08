namespace OpenConnector.CommandParserBinder.MetaData;

public class NotSupportShardingKeyGenerator:IShardingKeyGenerator
{
    public IComparable GenerateKey()
    {
        throw new NotImplementedException();
    }
}