using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Extensions;
using NCDC.ShardingRoute.Helpers;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

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