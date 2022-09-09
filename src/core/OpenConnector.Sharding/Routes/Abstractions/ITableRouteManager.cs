using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.Sharding.Routes.TableRoutes;

namespace OpenConnector.Sharding.Routes.Abstractions;

public interface ITableRouteManager
{
    bool HasRoute(string tableName);
    ITableRoute GetRoute(string tableName);
    ICollection<ITableRoute> GetRoutes();
    bool AddRoute(ITableRoute route);

    ICollection<TableRouteUnit> RouteTo(string tableName, DataSourceRouteResult dataSourceRouteResult,
        SqlParserResult sqlParserResult);
}