namespace ShardingConnector.ProtocolCore.Binaries;
/// <summary>
/// 二进制行
/// </summary>
public class BinaryRow
{
    /// <summary>
    /// 单元格
    /// </summary>
    private ICollection<BinaryCell> Cells { get; }
}