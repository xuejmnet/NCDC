using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;

namespace NCDC.Sharding.Routes.TableRoutes;

public sealed class TableRouteRuleContext
{
    public DataSourceRouteResult DataSourceRouteResult { get; }
    public SqlParserResult SqlParserResult { get; }

    public TableRouteRuleContext(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult)
    {
        DataSourceRouteResult = dataSourceRouteResult;
        SqlParserResult = sqlParserResult;
    }
}