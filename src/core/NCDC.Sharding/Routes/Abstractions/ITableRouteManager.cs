using NCDC.Sharding.Routes.DataSourceRoutes;
using NCDC.Sharding.Routes.TableRoutes;

namespace NCDC.Sharding.Routes.Abstractions;

public interface ITableRouteManager
{
    bool HasRoute(string tableName);
    ITableRoute GetRoute(string tableName);
    IReadOnlyCollection<ITableRoute> GetRoutes();
    bool AddRoute(ITableRoute route);

    ICollection<TableRouteUnit> RouteTo(string tableName, DataSourceRouteResult dataSourceRouteResult,
        SqlParserResult sqlParserResult);
}