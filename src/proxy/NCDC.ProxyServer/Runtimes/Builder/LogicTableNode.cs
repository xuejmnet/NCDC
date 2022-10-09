namespace NCDC.ProxyServer.Runtimes.Builder;

public class LogicTableNode
{
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    public string Name { get;  }
    /// <summary>
    /// 分库规则
    /// </summary>
    public string? ShardingDataSourceRule { get; }
    /// <summary>
    /// 分表规则
    /// </summary>
    public string? ShardingTableRule { get; }
    
    public LogicTableNode(string name, string? shardingDataSourceRule, string? shardingTableRule)
    {
        Name = name;
        ShardingDataSourceRule = shardingDataSourceRule;
        ShardingTableRule = shardingTableRule;
    }

}