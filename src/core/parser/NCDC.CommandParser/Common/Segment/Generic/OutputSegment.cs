using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class OutputSegment : ISqlSegment
{
    public OutputSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public TableNameSegment? TableName { get; set; }
    public ICollection<ColumnProjectionSegment> OutputColumns = new LinkedList<ColumnProjectionSegment>();
    public ICollection<ColumnSegment> TableColumns = new LinkedList<ColumnSegment>();
}