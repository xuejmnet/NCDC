using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NCDC.EntityFrameworkCore;
using NCDC.WebBootstrapper.Controllers.Passport.Login;
using NCDC.WebBootstrapper.Jwt;

namespace NCDC.WebBootstrapper.Controllers.Passport;

[ApiController]
[Route("/api/passport")]
public class PassportController: BaseApiController
{
    private readonly JwtIssuerOptions _jwtOptions;

    public PassportController(IOptions<JwtIssuerOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    [HttpPost,Route("login"),AllowAnonymous]
    public AppResult<string> Login(LoginRequest request)
    {
        if (request.Account != "admin" || request.Password != "123456")
        {
            return OutputFail<string>("用户名密码出错");
        }

        var token =GenerateJWT();
        return OutputOk(token);
    }
    private string GenerateJWT()
    {
        // 1. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 2. 定义需要使用到的Claims
        var claims = new[]
        {
            //sub user Id
            new Claim("uid", "a12345"),
            //role Admin
            new Claim("uname", "管理员"), 
        };

        // 3. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);
            
        // 5. 根据以上组件，生成token
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,    //Issuer
            _jwtOptions.Audience,  //Audience
            claims,                          //Claims,
            DateTime.Now,                    //notBefore
            DateTime.Now.AddDays(1),         //expires
            signingCredentials
        );
        // 6. 将token变为string
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtToken;
    }
}