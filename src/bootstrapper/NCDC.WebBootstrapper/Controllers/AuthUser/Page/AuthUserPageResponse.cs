namespace NCDC.WebBootstrapper.Controllers.AuthUser.Page;

public class AuthUserPageResponse
{
    
    public string Id { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = null!;
    /// <summary>
    /// 限制地址
    /// </summary>
    public string HostName { get; set; } = null!;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; set; }
}