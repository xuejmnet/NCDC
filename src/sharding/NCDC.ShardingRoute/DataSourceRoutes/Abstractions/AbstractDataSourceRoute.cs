using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.ShardingParser;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public abstract class AbstractDataSourceRoute:IDataSourceRoute
{
    private readonly IDataSourceRouteRule _dataSourceRouteRule;
    private readonly TableMetadata _tableMetadata;
    protected AbstractDataSourceRoute(ITableMetadataManager tableMetadataManager,IDataSourceRouteRule dataSourceRouteRule)
    {
        _dataSourceRouteRule = dataSourceRouteRule;
        // ReSharper disable once VirtualMemberCallInConstructor
        var tableName = TableName;
        _tableMetadata = tableMetadataManager.TryGet(tableName) ??
                         throw new ShardingConfigException($"cant find table metadata:{tableName}");
    }

    public abstract string TableName { get; }
    public TableMetadata GetTableMetadata()
    {
        return _tableMetadata;
    }

    public IDataSourceRouteRule GetRouteRule()
    {
        return _dataSourceRouteRule;
    }

    public abstract ICollection<string> Route(SqlParserResult sqlParserResult);

}