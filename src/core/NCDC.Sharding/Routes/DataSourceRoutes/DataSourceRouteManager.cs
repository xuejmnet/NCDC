using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public sealed class DataSourceRouteManager:IDataSourceRouteManager
{
    public bool HasRoute(string tableName)
    {
        throw new NotImplementedException();
    }

    public IDataSourceRoute GetRoute(string tableName)
    {
        throw new NotImplementedException();
    }

    public DataSourceRouteResult RouteTo(SqlParserResult sqlParserResult)
    {
        throw new NotImplementedException();
    }

    public bool AddDataSourceRoute(IDataSourceRoute dataSourceRoute)
    {
        throw new NotImplementedException();
    }
}