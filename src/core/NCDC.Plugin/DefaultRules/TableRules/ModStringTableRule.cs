using NCDC.Plugin.DefaultRules.ShardingRuleParams;
using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;

namespace NCDC.Plugin.DefaultRules.TableRules;

public class ModStringTableRule:AbstractTableRouteRule
{
    private readonly TableRuleParam _tableRuleParam;

    public ModStringTableRule(TableRuleParam tableRuleParam)
    {
        _tableRuleParam = tableRuleParam;
        if(tableRuleParam.TailLength<1)
            throw new ArgumentException($"{nameof(tableRuleParam.TailLength)} less than 1 ");
        if (tableRuleParam.Mod < 1)
            throw new ArgumentException($"{nameof(tableRuleParam.Mod)} less than 1 ");
        if (string.IsNullOrWhiteSpace(tableRuleParam.TableName))
            throw new ArgumentException($"{nameof(tableRuleParam.TableName)} is empty ");
        TableName = tableRuleParam.TableName;
        
    }
    public override string TableName { get; }
    protected override Func<string, bool> GetRouteToFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator)
    {
        var t = FormatToTail(shardingValue);
        var tableName = $"{TableName}{_tableRuleParam.TableSeparator}{t}";
        switch (shardingOperator)
        {
            case ShardingOperatorEnum.EQUAL: return tail => tail.Split('.')[1].Equals(tableName,StringComparison.OrdinalIgnoreCase);
            default:
            {
                return tail => true;
            }
        }
    }

    private string FormatToTail(IComparable shardingValue)
    {
        var hashCode = NCDCHelper.GetStringHashCode(shardingValue.ToString());
        return Math.Abs(hashCode % _tableRuleParam.Mod).ToString().PadLeft(_tableRuleParam.TailLength,'0');
    }

    public override void Configure(TableRuleConfigureBuilder builder)
    {
        builder.ShardingColumn(_tableRuleParam.ColumnName);
        if (_tableRuleParam.ColumnNames != null)
        {
            foreach (var columnName in _tableRuleParam.ColumnNames)
            {
                builder.AddShardingExtraColumn(columnName);
            }
        }
    }
}
