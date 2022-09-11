using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;

namespace NCDC.Sharding.Routes.TableRoutes;

public sealed class TableRouteContext
{
    public DataSourceRouteResult DataSourceRouteResult { get; }
    public SqlParserResult SqlParserResult { get; }

    public TableRouteContext(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult)
    {
        DataSourceRouteResult = dataSourceRouteResult;
        SqlParserResult = sqlParserResult;
    }
}