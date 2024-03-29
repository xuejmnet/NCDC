namespace NCDC.CommandParser.Common.Segment.DML.Expr;

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

}