namespace NCDC.Plugin;

public static class NCDCHelper
{
    
    /// <summary>
    /// c#默认的字符串gethashcode只是进程内一致如果程序关闭开启后那么就会乱掉所以这边建议重写string的gethashcode或者使用shardingcore提供的
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetStringHashCode(string? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(GetStringHashCode) + nameof(value));
        }
        int h = 0; // 默认值是0
        if (value.Length > 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
            }
        }
        return h;
    }
}