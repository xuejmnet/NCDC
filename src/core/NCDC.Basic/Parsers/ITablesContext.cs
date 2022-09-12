using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Segment.DML.Column;

namespace NCDC.Basic.Parsers;

public interface ITablesContext
{
    IEnumerable<string> GetTableNames();
    int GetTableNameCount();
    string? FindTableName(ColumnSegment column, TableMetadata tableMetadata);
}