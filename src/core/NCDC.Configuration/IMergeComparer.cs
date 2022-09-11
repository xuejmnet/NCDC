namespace NCDC.Configuration;

public interface IMergeComparer
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