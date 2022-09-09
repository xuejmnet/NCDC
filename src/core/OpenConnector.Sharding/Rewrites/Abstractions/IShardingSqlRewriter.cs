using OpenConnector.Sharding.Routes;
using OpenConnector.Sharding.Routes.Abstractions;

namespace OpenConnector.Sharding.Rewrites.Abstractions;

public interface IShardingSqlRewriter
{
    SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext);
}