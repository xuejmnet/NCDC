using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

/// <summary>
/// 数据源对象
/// </summary>
public class DataSourceEntity:BaseEntity
{
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string Database { get; set; }
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 数据源链接
    /// </summary>
    public string ConnectionString { get; set; }
    /// <summary>
    /// 是否默认数据源
    /// </summary>
    public bool IsDefault { get; set; }
}