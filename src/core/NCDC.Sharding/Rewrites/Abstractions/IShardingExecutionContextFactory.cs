using NCDC.Sharding.Executors.Context;
using NCDC.Sharding.Routes;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(RouteContext routeContext, SqlRewriteContext sqlRewriteContext);
}