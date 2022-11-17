using Microsoft.AspNetCore.Mvc;
using NCDC.WebBootstrapper.Controllers.User.Info;

namespace NCDC.WebBootstrapper.Controllers.User;

[ApiController]
[Route("/api/user")]
public class UserController:BaseApiController
{
    
    [HttpGet,Route("info")]
    public AppResult<UserInfoResponse> Info()
    {
        return OutputOk(new UserInfoResponse()
        {
            UserId = "a12345",
            Username = "管理员",
            Account = "admin"
        });
    }
}