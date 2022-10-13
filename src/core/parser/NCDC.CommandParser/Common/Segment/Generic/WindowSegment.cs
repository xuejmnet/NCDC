namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class WindowSegment:ISqlSegment
{
    public WindowSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
}