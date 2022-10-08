using NCDC.Basic.TableMetadataManagers;
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
    public string Database { get; set; } = null!;
    /// <summary>
    /// 分表规则
    /// </summary>
    public string? ShardingTableRule { get; set; }
    /// <summary>
    /// 分库规则
    /// </summary>
    public string? ShardingDataSourceRule { get; set; }
    // /// <summary>
    // /// <code>Dictionary<string, ColumnMetadata></code> 反序列化
    // /// </summary>
    // public string? ColumnSchema { get; set; }

    public void Check()
    {
        if(ShardingTableRule.IsNullOrWhiteSpace()&&ShardingDataSourceRule.IsNullOrWhiteSpace())
        {
            throw new ShardingInvalidOperationException($"data source:[{Database}],table:[{TableName}] {nameof(ShardingTableRule)},{nameof(ShardingDataSourceRule)} error.");
        }

    }

}