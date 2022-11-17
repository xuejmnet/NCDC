using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NCDC.WebBootstrapper.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BaseApiController:ControllerBase
{
        
    /// <summary>返回成功的方法</summary>
    /// <param name="data"></param>
    /// <param name="msg"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    protected virtual AppResult<T> OutputOk<T>(T? data = default, string? msg = null, int code = 0) => new AppResult<T>(code, data, msg);

    /// <summary>返回成功object</summary>
    /// <param name="data"></param>
    /// <param name="msg"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    protected virtual AppResult<object> OutputOk(object? data = default, string? msg = null, int code = 0) => this.OutputOk<object>(data, msg, code);

    /// <summary>返回失败的方法</summary>
    /// <param name="msg"></param>
    /// <param name="code"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    protected virtual AppResult<T> OutputFail<T>(string? msg, int code = -1, T? data = default) => new AppResult<T>(code, data, msg);

    /// <summary>返回失败object</summary>
    /// <param name="msg"></param>
    /// <param name="code"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    protected virtual AppResult<object> OutputFail(string? msg, int code = -1, object? data = default) => this.OutputFail<object>(msg, code, data);

}