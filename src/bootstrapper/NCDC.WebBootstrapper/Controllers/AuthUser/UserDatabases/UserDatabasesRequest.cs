using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.AuthUser.UserDatabases;

public class UserDatabasesRequest
{
    [Display(Name = "用户id"),Required(ErrorMessage = "{0}不能为空")]
    public string Id { get; set; }
}