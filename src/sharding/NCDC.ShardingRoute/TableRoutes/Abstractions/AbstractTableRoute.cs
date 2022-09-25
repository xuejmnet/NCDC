using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
using NCDC.Plugin;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public abstract class AbstractTableRoute:ITableRoute
{
    private readonly ITableRouteRule _tableRouteRule;
    private readonly TableMetadata _tableMetadata;
    protected AbstractTableRoute(ITableMetadataManager tableMetadataManager,ITableRouteRule tableRouteRule)
    {
        _tableRouteRule = tableRouteRule;
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

    public ITableRouteRule GetRouteRule()
    {
        return _tableRouteRule;
    }

    public abstract ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);

   
}