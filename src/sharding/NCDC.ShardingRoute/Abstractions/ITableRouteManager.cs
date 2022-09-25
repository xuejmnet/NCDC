using NCDC.Plugin;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;
using NCDC.ShardingRoute.TableRoutes;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ShardingRoute.Abstractions;

public interface ITableRouteManager
{
    bool HasRoute(string tableName);
    ITableRoute GetRoute(string tableName);
    IReadOnlyCollection<ITableRoute> GetRoutes();
    bool AddRoute(ITableRoute route);

    ICollection<TableRouteUnit> RouteTo(string tableName, DataSourceRouteResult dataSourceRouteResult,
        SqlParserResult sqlParserResult);
}