namespace NCDC.CommandParser.Common.Segment.Generic.Table;

public sealed class DeleteMultiTableSegment:ITableSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public ICollection<SimpleTableSegment> ActualDeleteTables = new LinkedList<SimpleTableSegment>();
    public ITableSegment? RelationTable { get; set; }
    public string? GetAlias()
    {
        return null;
    }

    public void SetAlias(AliasSegment? alias)
    {
    }
}