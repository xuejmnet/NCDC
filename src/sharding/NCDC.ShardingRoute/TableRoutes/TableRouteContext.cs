using NCDC.Plugin;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes;

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