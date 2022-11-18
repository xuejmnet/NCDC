using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

/// <summary>
/// 数据源对象
/// </summary>
public class ActualDatabaseEntity:BaseEntity
{
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    public string LogicDatabaseName { get; set; } = null!;
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSourceName { get; set; } = null!;
    /// <summary>
    /// 数据源链接
    /// </summary>
    public string ConnectionString { get; set; } = null!;
    /// <summary>
    /// 是否默认数据源
    /// </summary>
    public bool IsDefault { get; set; }
}