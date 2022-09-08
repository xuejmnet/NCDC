namespace OpenConnector.CommandParserBinder.MetaData;

public interface IShardingKeyGenerator
{
    IComparable GenerateKey();
}