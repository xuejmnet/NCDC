using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Expressions;

namespace NCDC.ShardingRoute.Helpers;

public static class ShardingHelper
{
    public static RoutePredicateExpression GetRoutePredicateExpression(SqlParserResult sqlParserResult, TableMetadata tableMetadata,
        Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter, bool shardingTableRoute)
    {
        var sqlRoutePredicateDiscover = new SqlRoutePredicateDiscover(tableMetadata,keyTranslateFilter,shardingTableRoute);
        return sqlRoutePredicateDiscover.GetRouteParseExpression(sqlParserResult);
    }
}