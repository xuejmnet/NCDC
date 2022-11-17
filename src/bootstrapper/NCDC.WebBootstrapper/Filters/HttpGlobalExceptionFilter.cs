using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NCDC.WebBootstrapper.Exceptions;
using Newtonsoft.Json;

namespace NCDC.WebBootstrapper.Filters
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 09 November 2020 08:49:16
* @Email: 326308290@qq.com
*/
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
            if (context.Exception is AppException appException)
            {
                var json = new
                {
                    code = appException.Code,
                    msg = appException.Message,
                    data=(object?)null
                };

                context.Result =new OkObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else if (context.Exception.GetType() == typeof(Newtonsoft.Json.JsonSerializationException)
                     || context.Exception.GetType() == typeof(Newtonsoft.Json.JsonReaderException) 
                     || context.Exception.GetType() == typeof(CryptographicException) 
                     || context.Exception.GetType() == typeof(Newtonsoft.Json.JsonException) 
                     || context.Exception.GetType() == typeof(System.Text.Json.JsonException))
            {
                var json = new
                {
                    code = 400,
                    msg = "请求数据无法被解析",
                    data=(object?)null
                };

                context.Result =new OkObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                var json = new
                {
                    code = 500,
                    msg = "未知异常",
                    data=(object?)null
                };

                context.Result =new OkObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            context.ExceptionHandled = true;
        }
    }
}