using NCDC.Plugin.DefaultRules.ShardingRuleParams;
using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;

namespace NCDC.Plugin.DefaultRules.TableRules;

public class ModIntTableRule : AbstractModNumberTableRule
{
    private readonly TableRuleParam _tableRuleParam;

    public ModIntTableRule(TableRuleParam tableRuleParam) : base(tableRuleParam)
    {
        _tableRuleParam = tableRuleParam;
    }

    protected override string FormatToTail(IComparable shardingValue)
    {
        var hashCode = (int)shardingValue;
        return Math.Abs(hashCode % _tableRuleParam.Mod).ToString().PadLeft(_tableRuleParam.TailLength,'0');
    }
}