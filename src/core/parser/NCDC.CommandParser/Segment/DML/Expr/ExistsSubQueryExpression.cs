using NCDC.CommandParser.Segment.DML.Expr.SubQuery;

namespace NCDC.CommandParser.Segment.DML.Expr;

public sealed class ExistsSubQueryExpression:IExpressionSegment
{
    public ExistsSubQueryExpression(int startIndex, int stopIndex,SubQuerySegment subQuery)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        SubQuery = subQuery;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public SubQuerySegment SubQuery { get; }
    public bool Not { get; set; }
}