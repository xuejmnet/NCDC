namespace NCDC.ProxyServer.Binaries;

/// <summary>
/// 二进制单元格
/// </summary>
public class BinaryCell
{
    /// <summary>
    /// 列CSharpLanguage类型
    /// </summary>
    public Type ClrType { get; }
    // /// <summary>
    // /// 列数据库类型
    // /// </summary>
    // public int ColumnType { get; }
    /// <summary>
    /// 数据
    /// </summary>
    public object? Data { get; }

    public BinaryCell(object? data, Type clrType)
    {
        Data = data;
        ClrType = clrType;
        // ColumnType = columnType;
    }
}