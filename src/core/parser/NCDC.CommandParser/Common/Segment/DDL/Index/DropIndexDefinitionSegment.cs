namespace NCDC.CommandParser.Common.Segment.DDL.Index;

public sealed class DropIndexDefinitionSegment:IAlterDefinitionSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public IndexSegment Index { get; }
    public DropIndexDefinitionSegment(int startIndex, int stopIndex,IndexSegment index)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Index = index;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Index)}: {Index}";
    }
}