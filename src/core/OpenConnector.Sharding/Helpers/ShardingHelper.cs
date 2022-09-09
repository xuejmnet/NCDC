using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Expressions;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding.Helpers;

public static class ShardingHelper
{
    public static RoutePredicateExpression GetRoutePredicateExpression(SqlParserResult sqlParserResult, TableMetadata tableMetadata,
        Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter, bool shardingTableRoute)
    {
        var sqlRoutePredicateDiscover = new SqlRoutePredicateDiscover(tableMetadata,keyTranslateFilter,shardingTableRoute);
        return sqlRoutePredicateDiscover.GetRouteParseExpression(sqlParserResult);
    }
}