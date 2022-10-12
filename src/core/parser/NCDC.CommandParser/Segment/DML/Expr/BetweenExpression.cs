namespace NCDC.CommandParser.Segment.DML.Expr;

public sealed class BetweenExpression:IExpressionSegment
{
    public BetweenExpression(int startIndex, int stopIndex,IExpressionSegment left,IExpressionSegment betweenExpr,IExpressionSegment andExpr,bool not)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Left = left;
        BetweenExpr = betweenExpr;
        AndExpr = andExpr;
        Not = not;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IExpressionSegment Left { get; }
    public IExpressionSegment BetweenExpr { get; }
    public IExpressionSegment AndExpr { get; }
    public bool Not { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Left)}: {Left}, {nameof(BetweenExpr)}: {BetweenExpr}, {nameof(AndExpr)}: {AndExpr}, {nameof(Not)}: {Not}";
    }
}