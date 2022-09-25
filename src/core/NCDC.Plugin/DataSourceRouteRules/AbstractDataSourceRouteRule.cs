using NCDC.Plugin.Enums;

namespace NCDC.Plugin.DataSourceRouteRules;

public abstract class AbstractDataSourceRouteRule:IDataSourceRouteRule
{
    public abstract string TableName { get; }

    public Func<string, bool> RouteFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator,
        string columnName, bool isMainShardingColumn)
    {
        if (isMainShardingColumn)
        {
            return GetRouteToFilter(shardingValue, shardingOperator);
        }

        return GetExtraRouteFilter(shardingValue, shardingOperator, columnName);
    }

    public abstract Func<string, bool> GetRouteToFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator);

    public virtual Func<string, bool> GetExtraRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        throw new NotImplementedException(columnName);
    }

    public ICollection<string> BeforeFilterDataSource(ICollection<string> allDataSource)
    {
        return allDataSource;
    }

    public ICollection<string> AfterFilterDataSource(ICollection<string> allDataSources, ICollection<string> beforeDataSources,
        ICollection<string> filterDataSources)
    {
        return filterDataSources;
    }
}