using NCDC.CommandParserBinder.MetaData;
using NCDC.Sharding.Configurations;
using NCDC.Sharding.Extensions;
using NCDC.Sharding.Helpers;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.Configuration;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractOperatorDataSourceRoute:AbstractFilterDataSourceRoute
{

    protected AbstractOperatorDataSourceRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    {
    }
    protected override ICollection<string> Route0(ICollection<string> dataSources, SqlParserResult sqlParserResult)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlParserResult,GetTableMetadata(),GetRouteFilter,false);
        var filter = routePredicateExpression.GetRoutePredicate();
        return dataSources.Where(o => filter(o)).ToList();
    }

    protected virtual Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        if (GetTableMetadata().IsMainShardingDataSourceColumn(columnName))
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
}