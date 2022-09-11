using NCDC.ShardingRewrite.Executors.Context;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(RouteContext routeContext, SqlRewriteContext sqlRewriteContext);
}