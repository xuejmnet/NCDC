using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ProxyServer.Contexts;

public sealed class RuntimeContextBuilder
{
    /// <summary>
    /// 数据库名称
    /// </summary>
    private readonly string _logicDatabase;

    private readonly ISet<Type> _tableRouteTypes = new HashSet<Type>();
    private readonly ISet<Type> _dataSourceRouteTypes = new HashSet<Type>();

    public RuntimeContextBuilder(string logicDatabase)
    {
        _logicDatabase = logicDatabase;
    }

    public RuntimeContextBuilder AddDataSourceRoute(Type dataSourceRouteType)
    {
        if (!typeof(IDataSourceRoute).IsAssignableFrom(dataSourceRouteType))
        {
            throw new InvalidOperationException($"{dataSourceRouteType.FullName} is not implement {nameof(IDataSourceRoute)}");
        }

        _dataSourceRouteTypes.Add(dataSourceRouteType);
        return this;
    }

    public RuntimeContextBuilder AddTableRoute(Type tableRouteType)
    {
        if (!typeof(ITableRoute).IsAssignableFrom(tableRouteType))
        {
            throw new InvalidOperationException($"{tableRouteType.FullName} is not implement {nameof(ITableRoute)}");
        }
        _tableRouteTypes.Add(tableRouteType);
        return this;
    }
}