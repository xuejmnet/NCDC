namespace ShardingConnector.ProxyServer.Binaries;
/// <summary>
/// 二进制行
/// </summary>
public class BinaryRow
{
    /// <summary>
    /// 单元格
    /// </summary>
    private List<BinaryCell> Cells { get; }
    public BinaryRow(List<BinaryCell> cells)
    {
        Cells = cells;
    }

}