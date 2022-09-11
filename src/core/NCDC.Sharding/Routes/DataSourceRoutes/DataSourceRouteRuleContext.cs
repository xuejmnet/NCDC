using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public sealed class DataSourceRouteRuleContext
{
    public SqlParserResult SqlParserResult { get; }

    public DataSourceRouteRuleContext(SqlParserResult sqlParserResult)
    {
        SqlParserResult = sqlParserResult;
    }
}