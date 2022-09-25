using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Extensions;
using NCDC.ShardingRoute.Helpers;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public  class ShardingDataSourceRoute:AbstractFilterDataSourceRoute
{

    public ShardingDataSourceRoute(ITableMetadataManager tableMetadataManager,IDataSourceRouteRule dataSourceRouteRule) : base(tableMetadataManager,dataSourceRouteRule)
    {
    }

    public override string TableName => GetRouteRule().TableName;
    protected override ICollection<string> Route0(ICollection<string> dataSources, SqlParserResult sqlParserResult)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlParserResult,GetTableMetadata(),GetRouteFilter,false);
        var filter = routePredicateExpression.GetRoutePredicate();
        return dataSources.Where(o => filter(o)).ToList();
    }

    protected virtual Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        var isMainShardingColumn = GetTableMetadata().IsMainShardingDataSourceColumn(columnName);
        return GetRouteRule().RouteFilter(shardingValue, shardingOperator, columnName, isMainShardingColumn);
    }

}