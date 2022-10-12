namespace NCDC.CommandParser.Segment.DML.Expr;

public sealed class InExpression:IExpressionSegment
{
    public InExpression(int startIndex, int stopIndex,IExpressionSegment left,IExpressionSegment right,bool not)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Left = left;
        Right = right;
        Not = not;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IExpressionSegment Left { get; }
    public IExpressionSegment Right { get; }
    public bool Not { get; }

    public ICollection<IExpressionSegment> GetExpressionList()
    {
        var result = new LinkedList<IExpressionSegment>();
        if (Right is ListExpression listExpression)
        {
            foreach (var expressionItem in listExpression.Items)
            {
                result.AddLast(expressionItem);
            }
        }
        else
        {
            result.AddLast(this);
        }

        return result;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Left)}: {Left}, {nameof(Right)}: {Right}, {nameof(Not)}: {Not}";
    }
}