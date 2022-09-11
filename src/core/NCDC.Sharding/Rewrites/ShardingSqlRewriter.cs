using NCDC.CommandParserBinder.MetaData;
using NCDC.Sharding.Rewrites.Abstractions;
using NCDC.Sharding.Rewrites.Token.SimpleObject;
using NCDC.Sharding.Routes;
using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Rewrites;

public sealed class ShardingSqlRewriter:IShardingSqlRewriter
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly IParameterRewriterBuilder _parameterRewriterBuilder;

    public ShardingSqlRewriter(ITableMetadataManager tableMetadataManager,IParameterRewriterBuilder parameterRewriterBuilder)
    {
        _tableMetadataManager = tableMetadataManager;
        _parameterRewriterBuilder = parameterRewriterBuilder;
    }
    public SqlRewriteContext Rewrite(SqlParserResult sqlParserResult, RouteContext routeContext)
    {
        var sqlRewriteContext = new SqlRewriteContext(_tableMetadataManager,routeContext);
        var parameterRewriters = _parameterRewriterBuilder.GetParameterRewriters(routeContext);
        foreach (var parameterRewriter in parameterRewriters)
        {
            if (!sqlRewriteContext.GetParameterContext().IsEmpty() && parameterRewriter.IsNeedRewrite(sqlRewriteContext.GetSqlCommandContext()))
            {
                parameterRewriter.Rewrite(sqlRewriteContext.GetParameterBuilder(), sqlRewriteContext.GetSqlCommandContext(), sqlRewriteContext.GetParameterContext());
            }
        }
        sqlRewriteContext.AddSqlTokenGenerators(new ShardingTokenGenerateBuilder(_tableMetadataManager).GetSqlTokenGenerators(routeContext));
        sqlRewriteContext.GenerateSqlTokens();
        return sqlRewriteContext;
    }
}