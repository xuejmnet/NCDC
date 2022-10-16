namespace NCDC.ShardingParser.Segment.Select.SubQuery;

public sealed class SubQueryTableContext
{
    public string TableName { get; }
    public string? Alias { get; }
    public ICollection<string> ColumnNames { get; }

    public SubQueryTableContext(string tableName,string? alias,ICollection<string> columnNames)
    {
        TableName = tableName;
        Alias = alias;
        ColumnNames = columnNames;
    }
}