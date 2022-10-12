using NCDC.CommandParser.Constant;

namespace NCDC.CommandParser.Segment.DDL.Coursor;

public sealed class DirectionSegment:ISqlSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public DirectionTypeEnum? DirectionType { get; set; }
    public long? Count { get; set; }
    public DirectionSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(DirectionType)}: {DirectionType}, {nameof(Count)}: {Count}";
    }
}