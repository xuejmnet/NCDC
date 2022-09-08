using System.Collections.Concurrent;
using System.Collections.Immutable;
using OpenConnector.Exceptions;

namespace OpenConnector.CommandParserBinder.MetaData;

public class TableMetadataManager:ITableMetadataManager
{
    private readonly ConcurrentDictionary<string, TableMetadata> _caches = new();
    public bool AddTableMetadata(TableMetadata tableMetadata)
    {
        return _caches.TryAdd(tableMetadata.LogicTableName, tableMetadata);
    }

    public bool IsShardingTable(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiTableMapping;
    }

    public bool IsOnlyShardingTable(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiTableMapping&&!tableMetadata.IsMultiDataSourceMapping;
    }

    public bool IsShardingDataSource(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiDataSourceMapping;
    }

    public bool IsShardingOnlyDataSource(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiDataSourceMapping&&!tableMetadata.IsMultiTableMapping;
    }

    public TableMetadata? TryGet(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return null;
        }

        return tableMetadata;
    }

    public bool IsSharding(string tableName)
    {
        if (!_caches.TryGetValue(tableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiTableMapping || tableMetadata.IsMultiDataSourceMapping;
    }

    public IReadOnlyList<string> GetAllShardingTableNames()
    {
        return _caches.Where(o => o.Value.IsMultiTableMapping || o.Value.IsMultiDataSourceMapping).Select(o => o.Key)
            .ToImmutableList();
    }

    public IReadOnlyList<string> GetAllColumnNames(string tableName)
    {
        var tableMetadata = TryGet(tableName);
        return tableMetadata?.Columns.Keys.ToList() ?? new List<string>(0);
    }

    public bool ContainsColumn(string tableName, string columnName)
    {
        var tableMetadata = TryGet(tableName);
        if (tableMetadata == null)
        {
            return false;
        }

        return tableMetadata.Columns.ContainsKey(columnName);
    }

    public bool IsShardingColumn(string tableName, string columnName)
    {
        var tableMetadata = TryGet(tableName);
        if (tableMetadata != null&&tableMetadata.Columns.ContainsKey(columnName))
        {
            return tableMetadata.IsShardingColumn(columnName);
        }
        return false;
    }

    public IComparable GetGenerateKey(string tableName)
    {
        var tableMetadata = TryGet(tableName);
        if (tableMetadata != null)
        {
            return tableMetadata.ShardingKeyGenerator.GenerateKey();
        }
        throw new ShardingException($"cant get generate key, table :{tableName}");
    }
}