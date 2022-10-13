namespace NCDC.CommandParser.Common.Segment.DDL.Charset;

public sealed class CharsetNameSegment:ISqlSegment
{
    public CharsetNameSegment(int startIndex, int stopIndex,string name)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Name = name;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public string Name { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Name)}: {Name}";
    }
}