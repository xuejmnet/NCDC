
namespace NCDC.Basic.TableMetadataManagers;

public interface ITableMetadataManager
{
    bool AddTableMetadata(TableMetadata tableMetadata);
    bool IsShardingTable(string tableName);
    bool IsOnlyShardingTable(string tableName);
    bool IsShardingDataSource(string tableName);
    bool IsShardingOnlyDataSource(string tableName);
    TableMetadata? TryGet(string tableName);
    bool IsSharding(string tableName);
    IReadOnlyList<string> GetAllShardingTableNames();
    IReadOnlyList<string> GetAllColumnNames(string tableName);
    bool ContainsColumn(string tableName, string columnName);
    bool IsShardingColumn(string tableName, string columnName);
}