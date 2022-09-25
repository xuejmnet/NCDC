using NCDC.Plugin.Enums;

namespace NCDC.Plugin.TableRouteRules;

public abstract class AbstractTableRouteRule:ITableRouteRule
{
    public abstract string TableName { get; }

    public virtual Func<string, bool> RouteFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator, string columnName,bool isMainShardingColumn)
    {
        if (isMainShardingColumn)
        {
            return GetRouteToFilter(shardingValue, shardingOperator);
        }

        return GetExtraRouteFilter(shardingValue, shardingOperator, columnName);
    }
   
    protected abstract Func<string, bool> GetRouteToFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator);

    protected virtual Func<string, bool> GetExtraRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        throw new NotImplementedException(columnName);
    }

    public virtual ICollection<string> BeforeFilterTableName(ICollection<string> allDataSource)
    {
        return allDataSource;
    }

    public virtual ICollection<TableRouteUnit> AfterFilterTableName(ICollection<string> allTableNames, ICollection<string> beforeTableNames, ICollection<TableRouteUnit> filterRouteUnits)
    {
        return filterRouteUnits;
    }
}