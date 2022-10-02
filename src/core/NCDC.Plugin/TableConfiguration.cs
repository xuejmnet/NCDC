namespace NCDC.Plugin;

public class TableConfiguration
{
    public string LogicTable { get; }

    public TableConfiguration(string logicTable)
    {
        LogicTable = logicTable;
        ShardingDataSourceColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        ShardingTableColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

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
    /// <exception cref="InvalidOperationException"></exception>
    public void SetShardingDataSourceColumn(string column)
    {
        if (ShardingDataSourceColumns.Contains(column))
            throw new InvalidOperationException($"same sharding data source column name:[{column}] repeat configure");
        ShardingDataSourceColumn = column;
        ShardingDataSourceColumns.Add(column);
    }

    /// <summary>
    /// 添加额外分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddExtraSharingDataSourceColumn(string column)
    {
        if (ShardingDataSourceColumns.Contains(column))
            throw new InvalidOperationException($"same sharding data source column name:[{column}] repeat configure");
        ShardingDataSourceColumns.Add(column);
    }

    /// <summary>
    /// 设置分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetShardingTableColumn(string column)
    {
        if (ShardingTableColumns.Contains(column))
            throw new InvalidOperationException($"same sharding table column name:[{column}] repeat configure");
        ShardingTableColumn = column;
        ShardingTableColumns.Add(column);
    }

    /// <summary>
    /// 添加额外分表字段
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddExtraSharingTableColumn(string column)
    {
        if (ShardingTableColumns.Contains(column))
            throw new InvalidOperationException($"same sharding table column name:[{column}] repeat configure");
        ShardingTableColumns.Add(column);
    }
}