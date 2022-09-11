
using NCDC.ShardingParser;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IShardingSqlRewriter
{
    SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext);
}