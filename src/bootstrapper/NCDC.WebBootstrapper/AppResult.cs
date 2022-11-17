namespace NCDC.WebBootstrapper;

public class AppResult<T>
{
    /// <summary>错误消息</summary>
    public string? Msg { get; }

    /// <summary>返回结果</summary>
    public T? Data { get; }

    /// <summary>返回状态码</summary>
    public int Code { get; }

    public AppResult(int code, T? data, string? msg)
    {
        this.Msg = msg;
        this.Code = code;
        this.Data = data;
    }
}