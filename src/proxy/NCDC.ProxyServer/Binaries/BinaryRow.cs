namespace NCDC.ProxyServer.Binaries;
/// <summary>
/// 二进制行
/// </summary>
public sealed class BinaryRow
{
    /// <summary>
    /// 单元格
    /// </summary>
    public List<BinaryCell> Cells { get; }
    public BinaryRow(List<BinaryCell> cells)
    {
        Cells = cells;
    }

    public static BinaryRow Empty { get; } = new BinaryRow(new List<BinaryCell>(0));

}