namespace OpenConnector.Sharding.ShardingComparisions;
/// <summary>
/// 分表内存排序比较器
/// </summary>
public interface IShardingComparer
{
    /// <summary>
    /// 比较 参数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="asc"></param>
    /// <returns></returns>
    int Compare(IComparable a, IComparable b,bool asc);
    /// <summary>
    /// 创建一个比较器
    /// </summary>
    /// <param name="comparerType"></param>
    /// <returns></returns>
    object CreateComparer(Type comparerType);
}