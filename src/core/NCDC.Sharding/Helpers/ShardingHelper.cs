using NCDC.Basic.Parser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Sharding.Expressions;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.Configuration;

namespace NCDC.Sharding.Helpers;

public static class ShardingHelper
{
    public static RoutePredicateExpression GetRoutePredicateExpression(SqlParserResult sqlParserResult, TableMetadata tableMetadata,
        Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter, bool shardingTableRoute)
    {
        var sqlRoutePredicateDiscover = new SqlRoutePredicateDiscover(tableMetadata,keyTranslateFilter,shardingTableRoute);
        return sqlRoutePredicateDiscover.GetRouteParseExpression(sqlParserResult);
    }
}