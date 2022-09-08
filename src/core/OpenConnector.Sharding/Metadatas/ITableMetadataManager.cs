// namespace OpenConnector.Sharding.Metadatas;
//
// public interface ITableMetadataManager
// {
//     bool AddTableMetadata(TableMetadata tableMetadata);
//     bool IsShardingTable(string tableName);
//     bool IsOnlyShardingTable(string tableName);
//     bool IsShardingDataSource(string tableName);
//     bool IsShardingOnlyDataSource(string tableName);
//     TableMetadata? TryGet(string tableName);
//     bool IsSharding(string tableName);
//     IReadOnlyList<string> GetAllShardingTableNames();
// }