// using System.Collections.Concurrent;
// using System.Collections.Immutable;
//
// namespace OpenConnector.Sharding.Metadatas;
//
// public class TableMetadataManager:ITableMetadataManager
// {
//     private readonly ConcurrentDictionary<string, TableMetadata> _caches = new();
//     public bool AddTableMetadata(TableMetadata tableMetadata)
//     {
//         return _caches.TryAdd(tableMetadata.LogicTableName, tableMetadata);
//     }
//
//     public bool IsShardingTable(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return false;
//         }
//
//         return tableMetadata.IsMultiTableMapping;
//     }
//
//     public bool IsOnlyShardingTable(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return false;
//         }
//
//         return tableMetadata.IsMultiTableMapping&&!tableMetadata.IsMultiDataSourceMapping;
//     }
//
//     public bool IsShardingDataSource(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return false;
//         }
//
//         return tableMetadata.IsMultiDataSourceMapping;
//     }
//
//     public bool IsShardingOnlyDataSource(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return false;
//         }
//
//         return tableMetadata.IsMultiDataSourceMapping&&!tableMetadata.IsMultiTableMapping;
//     }
//
//     public TableMetadata? TryGet(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return null;
//         }
//
//         return tableMetadata;
//     }
//
//     public bool IsSharding(string tableName)
//     {
//         if (!_caches.TryGetValue(tableName, out var tableMetadata))
//         {
//             return false;
//         }
//
//         return tableMetadata.IsMultiTableMapping || tableMetadata.IsMultiDataSourceMapping;
//     }
//
//     public IReadOnlyList<string> GetAllShardingTableNames()
//     {
//         return _caches.Where(o => o.Value.IsMultiTableMapping || o.Value.IsMultiDataSourceMapping).Select(o => o.Key)
//             .ToImmutableList();
//     }
// }