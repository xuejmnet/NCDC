namespace NCDC.CommandParserBinder.MetaData;

public class NotSupportShardingKeyGenerator:IShardingKeyGenerator
{
    public IComparable GenerateKey()
    {
        throw new NotImplementedException();
    }
}