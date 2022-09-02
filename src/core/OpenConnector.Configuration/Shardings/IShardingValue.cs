namespace OpenConnector.Shardings;

public interface IShardingValue
{
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    string LogicTableName { get; }
    /// <summary>
    /// 列名称
    /// </summary>
    string ColumnName { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    IComparable Value { get; }
}