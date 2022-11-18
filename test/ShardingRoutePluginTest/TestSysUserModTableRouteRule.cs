using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;

namespace ShardingRoutePluginTest;

public class TestSysUserModTableRouteRule:AbstractTableRouteRule
{
    public override string TableName => "sysusermod";
    protected override Func<string, bool> GetRouteToFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator)
    {
        var tail = ShardingKeyToTail(shardingValue);
        switch (shardingOperator)
        {
            case ShardingOperatorEnum.EQUAL: return dataSourceTable => dataSourceTable.EndsWith($"{TableName}_{tail}");
            default: return t => true;
        }
    }

    public override void Configure(TableRuleConfigureBuilder builder)
    {
        builder.ShardingColumn("id");
    }

    private string ShardingKeyToTail(IComparable shardingValue)
    {
        if (shardingValue is null)
        {
            throw new ArgumentNullException(TableName+"sharding value null");
        }

        return Math.Abs(GetStringHashCode(shardingValue.ToString()) % 3).ToString().PadLeft(2, '0');
    }
    private  int GetStringHashCode(string? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException();
        }
        int stringHashCode = 0;
        if (value.Length > 0)
        {
            for (int index = 0; index < value.Length; ++index)
                stringHashCode = 31 * stringHashCode + (int) value[index];
        }
        return stringHashCode;
    }
}