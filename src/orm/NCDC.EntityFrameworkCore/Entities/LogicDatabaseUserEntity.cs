using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

public class LogicDatabaseUserEntity:BaseEntity
{
    /// <summary>
    /// 所属数据库
    /// </summary>
    public string Database { get; set; } = null!;
    /// <summary>
    /// 授权用户账号
    /// </summary>
    public string UserName { get; set; } = null!;
}