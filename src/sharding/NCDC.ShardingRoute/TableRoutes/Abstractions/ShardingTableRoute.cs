using NCDC.Basic.Metadatas;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Plugin;
using NCDC.Plugin.Enums;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;
using NCDC.ShardingRoute.Extensions;
using NCDC.ShardingRoute.Helpers;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public  class ShardingTableRoute:AbstractFilterTableRoute
{
    public ShardingTableRoute(ITableRouteRule tableRouteRule,TableMetadata tableMetadata) : base(tableRouteRule,tableMetadata)
    {
    }

    protected override ICollection<TableRouteUnit> Route0(DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames, SqlParserResult sqlParserResult)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlParserResult,GetTableMetadata(),GetRouteFilter,true);
        var filter = routePredicateExpression.GetRoutePredicate();
        return FilterTableNameWithDataSourceResult(dataSourceRouteResult,beforeTableNames)
            .Where(o=> filter(o))
            .Select(ParseRouteWithTableName).ToList();
    }

    private  Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        var isMainShardingTableColumn = GetTableMetadata().IsMainShardingTableColumn(columnName);
        return GetRouteRule().RouteFilter(shardingValue,shardingOperator,columnName,isMainShardingTableColumn);
    }
    private  IEnumerable<string> FilterTableNameWithDataSourceResult(
        DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames)
    {
      
        return beforeTableNames.Where(tableName =>
            dataSourceRouteResult.IntersectDataSources.Contains(ParseDataSourceWithTableName(tableName)));
    }

    private string ParseDataSourceWithTableName(string tableName)
    {
        return tableName.Split(".")[0];
    }

    private TableRouteUnit ParseRouteWithTableName(string tableName)
    {
        if (!tableName.Contains("."))
        {
            throw new ShardingNotSupportedException($"not supported table name:{tableName}");
        }

        var tableInfos = tableName.Split(".");
        return new TableRouteUnit(tableInfos[0], TableName,tableInfos[1]);
    }
}