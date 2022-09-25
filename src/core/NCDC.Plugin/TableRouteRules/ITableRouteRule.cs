using NCDC.Plugin.Enums;

namespace NCDC.Plugin.TableRouteRules;

public interface ITableRouteRule:IRouteRule
{
    string TableName { get; }
    Func<string, bool> RouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName,bool isMainShardingColumn);

    ICollection<string> BeforeFilterTableName(ICollection<string> allDataSource);

    ICollection<TableRouteUnit> AfterFilterTableName(ICollection<string> allTableNames,
        ICollection<string> beforeTableNames,
        ICollection<TableRouteUnit> filterRouteUnits);
}