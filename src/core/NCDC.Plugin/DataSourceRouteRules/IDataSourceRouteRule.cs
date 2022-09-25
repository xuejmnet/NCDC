using NCDC.Plugin.Enums;

namespace NCDC.Plugin.DataSourceRouteRules;

public interface IDataSourceRouteRule:IRouteRule
{
    string TableName { get; }
    Func<string, bool> RouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName,bool isMainShardingColumn);

    ICollection<string> BeforeFilterDataSource(ICollection<string> allDataSource);

    ICollection<string> AfterFilterDataSource(ICollection<string> allDataSources,
        ICollection<string> beforeDataSources,
        ICollection<string> filterDataSources);
}