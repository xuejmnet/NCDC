using OpenConnector.Exceptions;

namespace OpenConnector.CommandParserBinder.MetaData;

public class TableMetadata
{
    public string LogicTableName { get; }

    /// <summary>
    /// ignore case key
    /// </summary>
    public Dictionary<string, ColumnMetadata> Columns { get; }

    public IShardingKeyGenerator ShardingKeyGenerator { get; private set; }

    public TableMetadata(string logicTableName, Dictionary<string, ColumnMetadata> columns)
    {
        LogicTableName = logicTableName;
        Columns = columns.ToDictionary(o => o.Key, o => o.Value, StringComparer.OrdinalIgnoreCase);
        ShardingTableColumns = new HashSet<string>();
        ShardingDataSourceColumns = new HashSet<string>();
        ShardingKeyGenerator = new NotSupportShardingKeyGenerator();
    }

    /// <summary>
    /// 是否多数据源
    /// </summary>
    public bool IsMultiDataSourceMapping => null != ShardingDataSourceColumn;

    /// <summary>
    /// 是否分表
    /// </summary>
    public bool IsMultiTableMapping => null != ShardingTableColumn;

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
    /// 分表隔离器 table sharding tail prefix
    /// </summary>
    public string TableSeparator { get; private set; } = "_";


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
    public void SetShardingTableProperty(string column)
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
    public void AddExtraSharingTableProperty(string column)
    {
        Check.NotNull(column, column);
        if (ShardingTableColumns.Contains(column))
            throw new ShardingConfigException($"same sharding table column name:[{column}] repeat configure");
        ShardingTableColumns.Add(column);
    }

    /// <summary>
    /// 分表表和后缀连接器
    /// </summary>
    /// <param name="separator"></param>
    public void SetTableSeparator(string separator)
    {
        TableSeparator = separator;
    }

    /// <summary>
    /// 启动时检查分库信息是否完整
    /// </summary>
    public void CheckShardingDataSourceMetadata()
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
    public void CheckShardingTableMetadata()
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
    public void CheckGenericMetadata()
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