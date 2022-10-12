using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Segment.DDL.Table;

public sealed class RenameTableDefinitionSegment:IAlterDefinitionSegment
{
    public RenameTableDefinitionSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public SimpleTableSegment? Table { get; set; }
    public SimpleTableSegment? RenameTable { get; set; }
}