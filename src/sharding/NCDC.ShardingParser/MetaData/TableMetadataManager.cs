using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Basic.Metadatas;
using NCDC.Exceptions;

namespace NCDC.ShardingParser.MetaData;

public class TableMetadataManager:ITableMetadataManager
{
    private readonly ConcurrentDictionary<string, TableMetadata> _caches = new();
    private readonly ConcurrentDictionary<string/*actual table name*/, string/*logic table name*/> _cacheIndex = new();
    public bool AddTableMetadata(TableMetadata tableMetadata)
    {
        if (_caches.TryAdd(tableMetadata.LogicTableName, tableMetadata))
        {
            var tableNames = tableMetadata.TableNames;
            foreach (var tableName in tableNames)
            {
                _cacheIndex.TryAdd(tableName, tableMetadata.LogicTableName);
            }
            return true;
        }

        return false;
    }

    public bool IsShardingTable(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiTableMapping;
    }

    public bool IsOnlyShardingTable(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiTableMapping&&!tableMetadata.IsMultiDataSourceMapping;
    }

    public bool IsShardingDataSource(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiDataSourceMapping;
    }

    public bool IsShardingOnlyDataSource(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            return false;
        }

        return tableMetadata.IsMultiDataSourceMapping&&!tableMetadata.IsMultiTableMapping;
    }

    public TableMetadata? TryGet(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            return null;
        }

        return tableMetadata;
    }

    public TableMetadata? TryGetByActualTableName(string actualTableName)
    {
        if (!_cacheIndex.TryGetValue(actualTableName, out var logicTableName))
        {
            return null;
        }

        return TryGet(logicTableName);
    }

    public TableMetadata Get(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
        {
            throw new ShardingException($"not found table metadata:[{logicTableName}]");
        }

        return tableMetadata;
    }

    public bool Contains(string logicTableName)
    {
        return _caches.ContainsKey(logicTableName);
    }

    public bool IsSharding(string logicTableName)
    {
        if (!_caches.TryGetValue(logicTableName, out var tableMetadata))
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

    public IReadOnlyList<string> GetAllColumnNames(string logicTableName)
    {
        var tableMetadata = TryGet(logicTableName);
        return tableMetadata?.Columns.Keys.ToList() ?? new List<string>(0);
    }

    public bool ContainsColumn(string logicTableName, string columnName)
    {
        var tableMetadata = TryGet(logicTableName);
        if (tableMetadata == null)
        {
            return false;
        }

        return tableMetadata.Columns.ContainsKey(columnName);
    }

    public bool IsShardingColumn(string logicTableName, string columnName)
    {
        var tableMetadata = TryGet(logicTableName);
        if (tableMetadata != null&&tableMetadata.Columns.ContainsKey(columnName))
        {
            return tableMetadata.IsShardingColumn(columnName);
        }
        return false;
    }
}