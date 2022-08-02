namespace ShardingConnector.ProtocolCore.Binaries;

/// <summary>
/// 二进制单元格
/// </summary>
public class BinaryCell
{
    /// <summary>
    /// 列类型
    /// </summary>
    public IBinaryColumnType ColumnType { get; }
    /// <summary>
    /// 数据
    /// </summary>
    public object? Data { get; }

    public BinaryCell(IBinaryColumnType columnType,object? data)
    {
        ColumnType = columnType;
        Data = data;
    }
}