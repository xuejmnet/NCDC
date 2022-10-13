namespace NCDC.CommandParser.Common.Segment.DML.Expr;

public sealed class ListExpression:IExpressionSegment
{
    public ListExpression(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Items = new LinkedList<IExpressionSegment>();
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ICollection<IExpressionSegment> Items { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Items)}: {Items}";
    }
}