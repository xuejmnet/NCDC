using NCDC.Plugin.DefaultRules.ShardingRuleParams;

namespace NCDC.Plugin.DefaultRules.TableRules;

public class ModLongTableRule: AbstractModNumberTableRule
{
    private readonly TableRuleParam _tableRuleParam;

    public ModLongTableRule(TableRuleParam tableRuleParam) : base(tableRuleParam)
    {
        _tableRuleParam = tableRuleParam;
    }

    protected override string FormatToTail(IComparable shardingValue)
    {
        var hashCode = (long)shardingValue;
        return Math.Abs(hashCode % _tableRuleParam.Mod).ToString().PadLeft(_tableRuleParam.TailLength,'0');
    }
}