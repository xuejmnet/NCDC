using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NCDC.WebBootstrapper.Filters
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 09 November 2020 09:16:03
* @Email: 326308290@qq.com
*/
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var validationErrors = context.ModelState
                .Keys
                .SelectMany(k => context.ModelState[k].Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var json = new
            {
                code = 400,
                msg = string.Join(",", validationErrors),
                data=(object?)null
            };
            context.Result = new OkObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        }

    }
}