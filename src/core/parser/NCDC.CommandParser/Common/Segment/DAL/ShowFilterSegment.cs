using NCDC.CommandParser.Common.Segment.DML.Predicate;

namespace NCDC.CommandParser.Common.Segment.DAL;

public sealed class ShowFilterSegment:ISqlSegment
{
    public ShowFilterSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ShowLikeSegment? Like { get; set; }
    public WhereSegment? Where { get; set; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Like)}: {Like}, {nameof(Where)}: {Where}";
    }
}