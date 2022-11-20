using NCDC.EntityFrameworkCore.Entities.Base;
using NCDC.Exceptions;
using NCDC.Extensions;

namespace NCDC.EntityFrameworkCore.Entities;

public class LogicTableEntity:BaseEntity
{
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    public string TableName { get; set; } = null!;
    public string LogicDatabaseId { get; set; } = null!;
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
    // /// <summary>
    // /// <code>Dictionary<string, ColumnMetadata></code> 反序列化
    // /// </summary>
    // public string? ColumnSchema { get; set; }

    public void Check()
    {
        if(TableRule.IsNullOrWhiteSpace()&&DataSourceRule.IsNullOrWhiteSpace())
        {
            throw new ShardingInvalidOperationException($"data source:[{LogicDatabaseId}],table:[{TableName}] {nameof(TableRule)},{nameof(DataSourceRule)} error.");
        }

    }

}