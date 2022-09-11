using OpenConnector.Sharding.Executors.Context;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.Abstractions;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(RouteContext routeContext, SqlRewriteContext sqlRewriteContext);
}