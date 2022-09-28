using System.Diagnostics.CodeAnalysis;
using NCDC.Exceptions;

namespace NCDC;

public static class Check
{
    /// <summary>
    /// 不能为空
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    public static T NotNull<T>(T value,string parameterName)
    {
        if(value==null)
            throw new ArgumentNullException(parameterName);

        return value;
    }
    /// <summary>
    /// 应该为空
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    public static T ShouldNull<T>(T value, string parameterName)
    {
        if (value != null)
            throw new ArgumentNullException(parameterName);

        return value;
    }

    /// <summary>
    /// 判断是否是V类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="error"></param>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    /// <exception cref="ShardingAssertException"></exception>
    public static V ObjectIsType<V>(object value, string error)
    {
        if (value is V v)
        {
            return v;
        }

        throw new ShardingAssertException(error);
    }
}