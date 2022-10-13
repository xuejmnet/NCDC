using NCDC.CommandParser.Common.Segment.DML.Expr.Complex;

namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class WithSegment:ISqlSegment
{
    public WithSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }

    public ICollection<CommonTableExpressionSegment> CommonTableExpressions =
        new LinkedList<CommonTableExpressionSegment>();
}