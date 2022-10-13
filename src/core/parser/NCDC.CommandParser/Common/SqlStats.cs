using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common;

public sealed class SqlStats
{
    public IDictionary<string, SimpleTableSegment> Tables = new SortedDictionary<string, SimpleTableSegment>();
    public IDictionary<int, ColumnSegment> Columns = new SortedDictionary<int, ColumnSegment>();

    public void AddTable(SimpleTableSegment tableSegment)
    {
        var table = tableSegment.TableName.IdentifierValue.Value;
        Tables.TryAdd(table, tableSegment);
    }

    public void AddColumn(ColumnSegment columnSegment)
    {
        var hashCode = columnSegment.GetHashCode();
        Columns.TryAdd(hashCode, columnSegment);
    }
}