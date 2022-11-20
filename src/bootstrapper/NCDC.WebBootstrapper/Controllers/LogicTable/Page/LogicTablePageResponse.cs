using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.WebBootstrapper.Controllers.LogicTable.Page;

public class LogicTablePageResponse
{
    public string Id { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    public string TableName { get; set; } = null!;
    public string LogicDatabaseName { get; set; } = null!;
    /// <summary>
    /// 分库规则
    /// </summary>
    public string? DataSourceRule { get; set; }
    public string? DataSourceRuleParam { get; set; }
    /// <summary>
    /// 分表规则
    /// </summary>
    public string? TableRule { get; set; }
    public string? TableRuleParam { get; set; }
}