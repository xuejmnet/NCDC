using System.Runtime.Serialization;

namespace NCDC.WebBootstrapper.Exceptions;

public class AppException:Exception
{
    public int Code { get; }
    /// <summary>
    /// 当异常名称不确定，但是异常code确定时调用此方法
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public AppException(int code, string? message) : base(message)
    {
        Code = code;

    }
    public AppException(string? message) : base(message)
    {
        Code = 400;
    }
}