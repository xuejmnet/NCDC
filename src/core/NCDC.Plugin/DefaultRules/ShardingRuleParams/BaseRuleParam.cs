namespace NCDC.Plugin.DefaultRules.ShardingRuleParams;

public class BaseRuleParam
{
    public string TableName { get; set; } = null!;
    public string ColumnName { get; set; } = null!;
    public List<string>? ColumnNames { get; set; }
}