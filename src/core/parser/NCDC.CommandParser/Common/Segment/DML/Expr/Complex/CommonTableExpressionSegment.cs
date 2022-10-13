using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DML.Expr.Complex;

public sealed class CommonTableExpressionSegment:IExpressionSegment
{
    public CommonTableExpressionSegment(int startIndex, int stopIndex,IdentifierValue identifierValue,SubQuerySegment subQuery)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        IdentifierValue = identifierValue;
        SubQuery = subQuery;
        Columns = new LinkedList<ColumnSegment>();
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IdentifierValue IdentifierValue { get; }
    public SubQuerySegment SubQuery { get; }
    public ICollection<ColumnSegment> Columns { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(IdentifierValue)}: {IdentifierValue}, {nameof(SubQuery)}: {SubQuery}, {nameof(Columns)}: {Columns}";
    }
}