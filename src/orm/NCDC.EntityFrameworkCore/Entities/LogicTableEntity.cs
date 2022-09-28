using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Entities;

public class LogicTableEntity:BaseEntity
{
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    public string LogicName { get; set; } = null!;
    public string Database { get; set; } = null!;
    /// <summary>
    /// 逗号分割第一个表示为主的分表字段
    /// </summary>
    public string? ShardingTableColumns { get; set; }
    /// <summary>
    /// 逗号分割第一个表示为主的分库字段
    /// </summary>
    public string? ShardingDataSourceColumns { get; set; }

}