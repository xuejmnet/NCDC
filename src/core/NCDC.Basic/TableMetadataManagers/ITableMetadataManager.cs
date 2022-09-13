
namespace NCDC.Basic.TableMetadataManagers;

public interface ITableMetadataManager
{
    bool AddTableMetadata(TableMetadata tableMetadata);
    bool IsShardingTable(string logicTableName);
    bool IsOnlyShardingTable(string logicTableName);
    bool IsShardingDataSource(string logicTableName);
    bool IsShardingOnlyDataSource(string logicTableName);
    TableMetadata? TryGet(string logicTableName);
    TableMetadata? TryGetByActualTableName(string actualTableName);
    TableMetadata Get(string logicTableName);
    bool Contains(string logicTableName);
    bool IsSharding(string logicTableName);
    IReadOnlyList<string> GetAllShardingTableNames();
    IReadOnlyList<string> GetAllColumnNames(string logicTableName);
    bool ContainsColumn(string logicTableName, string columnName);
    bool IsShardingColumn(string logicTableName, string columnName);
}