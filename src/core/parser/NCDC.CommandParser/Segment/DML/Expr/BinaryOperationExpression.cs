namespace NCDC.CommandParser.Segment.DML.Expr;

public sealed class BinaryOperationExpression:IExpressionSegment
{
    public BinaryOperationExpression(int startIndex, int stopIndex,IExpressionSegment left,IExpressionSegment right,string @operator,string text)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Left = left;
        Right = right;
        Operator = @operator;
        Text = text;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IExpressionSegment Left { get; }
    public IExpressionSegment Right { get; }
    public string Operator { get; }
    public string Text { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Left)}: {Left}, {nameof(Right)}: {Right}, {nameof(Operator)}: {Operator}, {nameof(Text)}: {Text}";
    }
}