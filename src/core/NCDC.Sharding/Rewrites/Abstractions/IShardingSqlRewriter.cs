using NCDC.Sharding.Routes;
using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IShardingSqlRewriter
{
    SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext);
}