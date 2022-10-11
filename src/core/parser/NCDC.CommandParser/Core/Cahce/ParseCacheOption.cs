namespace NCDC.CommandParser.Core.Cahce;

public class ParseCacheOption
{
    public int InitialCapacity { get; }
    public long MaximumSize { get; }

    public ParseCacheOption(int initialCapacity,long maximumSize)
    {
        InitialCapacity = initialCapacity;
        MaximumSize = maximumSize;
    }
}