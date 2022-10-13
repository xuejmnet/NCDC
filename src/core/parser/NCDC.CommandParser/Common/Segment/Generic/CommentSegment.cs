namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class CommentSegment:ISqlSegment
{
    public CommentSegment(int startIndex, int stopIndex,string text)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Text = text;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public string Text { get; }
}