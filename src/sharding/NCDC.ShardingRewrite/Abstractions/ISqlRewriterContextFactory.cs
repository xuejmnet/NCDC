
using NCDC.ShardingParser;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.Abstractions;

public interface ISqlRewriterContextFactory
{
    SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext);
}