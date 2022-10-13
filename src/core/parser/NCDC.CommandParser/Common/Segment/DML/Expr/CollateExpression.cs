using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;

namespace NCDC.CommandParser.Common.Segment.DML.Expr;

public sealed class CollateExpression:IExpressionSegment
{
    public CollateExpression(int startIndex, int stopIndex,ISimpleExpressionSegment collateName)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        CollateName = collateName;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ISimpleExpressionSegment CollateName { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(CollateName)}: {CollateName}";
    }
}