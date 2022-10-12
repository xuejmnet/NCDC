using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.DML.Expr;

namespace NCDC.CommandParser.Segment.DML.Assignment;

public sealed class ColumnAssignmentSegment:AssignmentSegment
{
    private readonly List<ColumnSegment> _columns;
    private readonly IExpressionSegment _value;
    public override int StartIndex { get; }
    public override int StopIndex { get; }

    public ColumnAssignmentSegment(int startIndex, int stopIndex,List<ColumnSegment> columns,IExpressionSegment value)
    {
        _columns = columns;
        _value = value;
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public override List<ColumnSegment> GetColumns()
    {
        return _columns;
    }

    public override IExpressionSegment GetValue()
    {
        return _value;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Columns: {_columns}, Value: {_value}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
    }
}