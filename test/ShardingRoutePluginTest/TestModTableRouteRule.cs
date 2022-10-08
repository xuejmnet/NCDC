using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;

namespace ShardingRoutePluginTest;

public class TestModTableRouteRule : AbstractTableRouteRule
{
    // public TestModTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    // {
    // }
    //
    // public override string TableName => "sysusermod";
    //
    // public override Func<string, bool> GetRouteToFilter(IComparable shardingValue,
    //     ShardingOperatorEnum shardingOperator)
    // {
    //     var tail = FormatTableName(shardingValue);
    //     var table = $"{TableName}{tail}";
    //
    //     switch (shardingOperator)
    //     {
    //         case ShardingOperatorEnum.EQUAL: return t => t.EndsWith(table);
    //         default:
    //         {
    //             return t => true;
    //         }
    //     }
    // }
    //
    // public string FormatTableName(IComparable shardingValue)
    // {
    //     var shardingKey = $"{shardingValue}";
    //     var stringHashCode = GetStringHashCode(shardingKey) % 3;
    //     return stringHashCode.ToString().PadLeft(2, '0');
    // }
    //
    // public static int GetStringHashCode(string value)
    // {
    //     Check.NotNull(value, nameof(value));
    //     int h = 0; // 默认值是0
    //     if (value.Length > 0)
    //     {
    //         for (int i = 0; i < value.Length; i++)
    //         {
    //             h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
    //         }
    //     }
    //
    //     return h;
    // }
    public override string TableName=> "sysusermod";
    protected override Func<string, bool> GetRouteToFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator)
    {
        var tail = FormatTableName(shardingValue);
        var table = $"{TableName}_{tail}";
        
        switch (shardingOperator)
        {
            case ShardingOperatorEnum.EQUAL: return t => t.EndsWith(table);
            default:
            {
                return t => true;
            }
        }
    }
    private string FormatTableName(IComparable shardingValue)
    {
        var shardingKey = $"{shardingValue}";
        var stringHashCode = GetStringHashCode(shardingKey) % 3;
        return stringHashCode.ToString().PadLeft(2, '0');
    }
    private static int GetStringHashCode(string value)
    {
        int h = 0; // 默认值是0
        if (value.Length > 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
            }
        }
    
        return h;
    }
    public override void Configure(TableRuleConfigureBuilder builder)
    {
        builder.ShardingColumn("id");
    }
}