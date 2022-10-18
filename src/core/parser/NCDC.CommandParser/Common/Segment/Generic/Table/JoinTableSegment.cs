using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.Generic.Table;

public  sealed class JoinTableSegment:ITableSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public ITableSegment Left { get;  }
    public string JoinType { get; }
    public ITableSegment Right { get;  } 
    public IExpressionSegment? Condition { get; set; }
    public ICollection<ColumnSegment> Using = new LinkedList<ColumnSegment>();
    private AliasSegment? _alias;

    public JoinTableSegment(int startIndex,int stopIndex,ITableSegment left,ITableSegment right,string joinType)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Right = right;
        JoinType = joinType;
        Left = left;
    }
    public string? GetAlias()
    {
        return _alias?.IdentifierValue.Value;
    }

    public void SetAlias(AliasSegment? alias)
    {
        _alias = alias;
    }
}