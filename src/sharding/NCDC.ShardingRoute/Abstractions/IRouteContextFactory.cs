using NCDC.ShardingParser;

namespace NCDC.ShardingRoute.Abstractions;

public interface IRouteContextFactory
{
    RouteContext Create(SqlParserResult sqlParserResult);
}