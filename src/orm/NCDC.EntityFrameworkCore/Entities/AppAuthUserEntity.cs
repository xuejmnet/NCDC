using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

/// <summary>
/// 应用授权用户
/// </summary>
public class AppAuthUserEntity:BaseEntity
{
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