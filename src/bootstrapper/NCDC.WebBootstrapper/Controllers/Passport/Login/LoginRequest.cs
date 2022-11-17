using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.Passport.Login;

public class LoginRequest
{
    [Display(Name = "账号"),Required(ErrorMessage = "{0}不能为空")]
    public string Account { get; set; } =null!;
    [Display(Name = "密码"),Required(ErrorMessage = "{0}不能为空")]
    public string Password { get; set; } =null!;
}