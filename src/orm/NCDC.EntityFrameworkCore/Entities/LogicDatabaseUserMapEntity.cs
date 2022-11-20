using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

public class LogicDatabaseUserMapEntity:BaseEntity
{
    /// <summary>
    /// 所属数据库
    /// </summary>
    public string DatabaseId { get; set; } = null!;
    /// <summary>
    /// 授权用户账号
    /// </summary>
    public string AppAuthUserId { get; set; } = null!;
}