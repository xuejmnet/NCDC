using Newtonsoft.Json;

namespace NCDC.WebBootstrapper.Controllers.AuthUser.All;

public class AuthUserAllResponse
{
    public string Id { get; set; }
    [JsonProperty("userName")]
    public string UserNameDisplay => UserName +"--"+ (IsEnable ? "启用" : "禁用");
    [JsonIgnore]
    public string UserName { get; set; }
    [JsonIgnore]
    public bool IsEnable { get; set; }
}