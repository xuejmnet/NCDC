using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

public class ActualTableEntity:BaseEntity
{
    /// <summary>
    /// 表名
    /// </summary>
    public string TableName { get; set; } = null!;
    /// <summary>
    /// 数据库
    /// </summary>
    public string LogicDatabaseId { get; set; } = null!;
    /// <summary>
    /// 逻辑表名
    /// </summary>
    public string LogicTableId { get; set; } = null!;
    /// <summary>
    /// 数据源
    /// </summary>
    public string DataSource { get; set; } = null!;

}