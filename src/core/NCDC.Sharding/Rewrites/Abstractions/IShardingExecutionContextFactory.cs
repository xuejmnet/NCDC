using NCDC.Sharding.Executors.Context;
using NCDC.Sharding.Routes;

namespace NCDC.Sharding.Rewrites.Abstractions;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(RouteContext routeContext, SqlRewriteContext sqlRewriteContext);
}