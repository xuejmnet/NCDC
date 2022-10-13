using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Predicate;

public sealed class HavingSegment:ISqlSegment
{
    public HavingSegment(int startIndex, int stopIndex, IExpressionSegment expr)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Expr = expr;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IExpressionSegment Expr { get; }
}