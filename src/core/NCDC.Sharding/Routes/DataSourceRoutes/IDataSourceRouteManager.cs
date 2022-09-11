using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRouteManager
{
    bool HasRoute(string tableName);
    IDataSourceRoute GetRoute(string tableName);
    ICollection<string> RouteTo(string tableName,SqlParserResult sqlParserResult);
    IReadOnlyCollection<IDataSourceRoute> GetRoutes();
    /// <summary>
    /// 添加分库路由
    /// </summary>
    /// <param name="dataSourceRoute"></param>
    /// <returns></returns>
    bool AddDataSourceRoute(IDataSourceRoute dataSourceRoute);
}