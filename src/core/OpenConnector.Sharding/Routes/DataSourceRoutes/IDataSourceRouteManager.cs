using OpenConnector.Sharding.Routes.Abstractions;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRouteManager
{
    bool HasRoute(string tableName);
    IDataSourceRoute GetRoute(string tableName);
    DataSourceRouteResult RouteTo(SqlParserResult sqlParserResult);
    /// <summary>
    /// 添加分库路由
    /// </summary>
    /// <param name="dataSourceRoute"></param>
    /// <returns></returns>
    bool AddDataSourceRoute(IDataSourceRoute dataSourceRoute);
}