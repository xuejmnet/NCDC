using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.Generic.Table;

public  sealed class JoinTableSegment:ITableSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public string? JoinType { get; set; }
    public ITableSegment? Right { get; set; }
    public IExpressionSegment? Condition { get; set; }
    public ICollection<ColumnSegment> Using = new LinkedList<ColumnSegment>();
    private AliasSegment? _alias;
    public string? GetAlias()
    {
        return _alias?.IdentifierValue.Value;
    }

    public void SetAlias(AliasSegment alias)
    {
        _alias = alias;
    }
}