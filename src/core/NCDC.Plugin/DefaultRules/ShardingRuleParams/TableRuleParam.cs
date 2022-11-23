namespace NCDC.Plugin.DefaultRules.ShardingRuleParams;

public class TableRuleParam:BaseRuleParam
{
    public int Mod { get; set; }
    public int TailLength { get; set; }
    public string TableSeparator { get; set; } = "_";
}