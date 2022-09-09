using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Rewrites.Abstractions;
using OpenConnector.Sharding.Routes;
using OpenConnector.Sharding.Routes.Abstractions;

namespace OpenConnector.Sharding.Rewrites;

public sealed class ShardingSqlRewriter:IShardingSqlRewriter
{
    private readonly ITableMetadataManager _tableMetadataManager;

    public ShardingSqlRewriter(ITableMetadataManager tableMetadataManager)
    {
        _tableMetadataManager = tableMetadataManager;
    }
    public SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext)
    {
        throw new NotImplementedException();
    }
}