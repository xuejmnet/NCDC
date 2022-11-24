using System.Collections.Immutable;
using NCDC.Exceptions;

namespace NCDC.Basic.Metadatas;

public class TableMetadata
{
    public string LogicTableName { get; }

    /// <summary>
    /// ignore case key
    /// </summary>
    public IReadOnlyDictionary<string, ColumnMetadata> Columns { get; }


    public TableMetadata(string logicTableName, Dictionary<string, ColumnMetadata> columns)
    {
        LogicTableName = logicTableName;
        Columns = columns.ToImmutableDictionary(o => o.Key, o => o.Value, StringComparer.OrdinalIgnoreCase);
        ShardingTableColumns = new HashSet<string>();
        ShardingDataSourceColumns = new HashSet<string>();
    }

    private readonly HashSet<string> _dataSources = new ();
    private readonly HashSet<string> _tableNames = new ();
    public ICollection<string> DataSources => _dataSources.ToImmutableHashSet();
    public ICollection<string> TableNames => _tableNames.ToImmutableHashSet();

    public void AddActualTableWithDataSource(string dataSource, string actualTableName)
    {
        Check.NotNull(actualTableName, nameof(actualTableName));
        if (actualTableName.Contains("."))
        {
            throw new ShardingInvalidOperationException(
                $"{nameof(TableMetadata)}.{nameof(AddActualTableWithDataSource)} {nameof(actualTableName)}:{actualTableName} contains '.'");
        }
        if (!_dataSources.Contains(dataSource))
        {
            _dataSources.Add(dataSource);
        }

        var tableName = $"{dataSource}.{actualTableName}";
        if (!_tableNames.Contains(tableName))
        {
            _tableNames.Add(tableName);
        }
    }

    /// <summary>
    /// 是否多数据源
    /// </summary>
    public bool IsMultiDataSourceMapping => null != ShardingDataSourceColumn;

    /// <summary>
    /// 是否分表
    /// </summary>
    public bool IsMultiTableMapping => null != ShardingTableColumn;

    public bool IsSharding => IsMultiDataSourceMapping || IsMultiTableMapping;

    /// <summary>
    /// 分库字段
    /// </summary>
    public string? ShardingDataSourceColumn { get; private set; }

    /// <summary>
    /// 分库所有字段包括 ShardingDataSourceColumn
    /// </summary>
    public ISet<string> ShardingDataSourceColumns { get; }

    /// <summary>
    /// 分表字段 sharding table property
    /// </summary>
    public string? ShardingTableColumn { get; private set; }

    /// <summary>
    /// 分表所有字段包括 ShardingTableColumn
    /// </summary>
    public ISet<string> ShardingTableColumns { get; }
    


    /// <summary>
    /// 设置分库字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="ShardingConfigException"></exception>
    public void SetShardingDataSourceColumn(string column)
    {
        Check.NotNull(column, column);
        if (ShardingDataSourceColumns.Contains(column))
            throw new ShardingConfigException($"same sharding data source column name:[{column}] repeat configure");
        ShardingDataSourceColumn = column;
        ShardingDataSourceColumns.Add(column);
    }

    /// <summary>
    /// 添加额外分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="ShardingConfigException"></exception>
    public void AddExtraSharingDataSourceColumn(string column)
    {
        Check.NotNull(column, column);
        if (ShardingDataSourceColumns.Contains(column))
            throw new ShardingConfigException($"same sharding data source column name:[{column}] repeat configure");
        ShardingDataSourceColumns.Add(column);
    }

    /// <summary>
    /// 设置分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="ShardingConfigException"></exception>
    public void SetShardingTableColumn(string column)
    {
        Check.NotNull(column, column);
        if (ShardingTableColumns.Contains(column))
            throw new ShardingConfigException($"same sharding table column name:[{column}] repeat configure");
        ShardingTableColumn = column;
        ShardingTableColumns.Add(column);
    }

    /// <summary>
    /// 添加额外分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="ShardingConfigException"></exception>
    public void AddExtraSharingTableColumn(string column)
    {
        Check.NotNull(column, column);
        if (ShardingTableColumns.Contains(column))
            throw new ShardingConfigException($"same sharding table column name:[{column}] repeat configure");
        ShardingTableColumns.Add(column);
    }

    public void CheckMetadata()
    {
        CheckGenericMetadata();
        CheckShardingDataSourceMetadata();
        CheckShardingTableMetadata();
    }
    /// <summary>
    /// 启动时检查分库信息是否完整
    /// </summary>
    private void CheckShardingDataSourceMetadata()
    {
        if (!IsMultiDataSourceMapping)
        {
            throw new ShardingConfigException($"not found  table :{LogicTableName} configure");
        }

        if (ShardingDataSourceColumn == null)
        {
            throw new ShardingConfigException($"not found  table :{LogicTableName} configure sharding column");
        }
    }

    /// <summary>
    /// 启动时检查分表信息是否完整
    /// </summary>
    private void CheckShardingTableMetadata()
    {
        if (!IsMultiTableMapping)
        {
            throw new ShardingConfigException($"not found  table :{LogicTableName} configure");
        }

        if (ShardingTableColumn == null)
        {
            throw new ShardingConfigException($"not found  table :{LogicTableName} configure sharding column");
        }
    }

    /// <summary>
    /// 启动时检查对象信息是否完整
    /// </summary>
    private void CheckGenericMetadata()
    {
        if (!IsMultiTableMapping && !IsMultiDataSourceMapping)
        {
            throw new ShardingConfigException($"not found  table:{LogicTableName} configure");
        }
    }


    public bool IsShardingColumn(string columnName)
    {
        if (ContainsColumn(columnName))
        {
            return ShardingTableColumns.Contains(columnName) ||
                   ShardingDataSourceColumns.Contains(columnName);
        }

        return false;
    }

    public bool IsShardingColumn(string columnName, bool isShardingTable)
    {
        if (ContainsColumn(columnName))
        {
            if (isShardingTable)
            {
                return ShardingTableColumns.Contains(columnName);
            }

            return ShardingDataSourceColumns.Contains(columnName);
        }

        return false;
    }

    public bool ContainsColumn(string columnName)
    {
        return Columns.ContainsKey(columnName);
    }
}