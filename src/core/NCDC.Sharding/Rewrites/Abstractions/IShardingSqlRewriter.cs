using NCDC.Sharding.Routes;
using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Rewrites.Abstractions;

public interface IShardingSqlRewriter
{
    SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext);
}