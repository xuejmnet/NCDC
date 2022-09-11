namespace NCDC.CommandParserBinder.MetaData;

public interface IShardingKeyGenerator
{
    IComparable GenerateKey();
}