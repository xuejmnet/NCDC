using NCDC.ShardingParser;

namespace NCDC.ShardingRoute.DataSourceRoutes;

public sealed class DataSourceRouteRuleContext
{
    public SqlParserResult SqlParserResult { get; }

    public DataSourceRouteRuleContext(SqlParserResult sqlParserResult)
    {
        SqlParserResult = sqlParserResult;
    }
}