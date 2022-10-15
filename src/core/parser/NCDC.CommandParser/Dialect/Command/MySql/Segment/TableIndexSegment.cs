using NCDC.CommandParser.Common.Segment;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.Segment;

public abstract class TableIndexSegment:ISqlSegment
{
    protected TableIndexSegment(int startIndex, int stopIndex, SimpleTableSegment table)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        Table = table;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public SimpleTableSegment Table { get; }
}