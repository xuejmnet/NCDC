namespace NCDC.CommandParser.Common.Segment.DML.Expr;

public sealed class NotExpression:IExpressionSegment
{
    public NotExpression(int startIndex, int stopIndex,IExpressionSegment expression)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Expression = expression;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IExpressionSegment Expression { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Expression)}: {Expression}";
    }
}