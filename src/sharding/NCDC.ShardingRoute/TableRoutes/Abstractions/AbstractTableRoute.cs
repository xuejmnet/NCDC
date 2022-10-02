using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
using NCDC.Plugin;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public abstract class AbstractTableRoute:ITableRoute
{
    private readonly ITableRouteRule _tableRouteRule;
    private  readonly TableMetadata _tableMetadata;
    protected AbstractTableRoute(ITableRouteRule tableRouteRule,TableMetadata tableMetadata)
    {
        _tableRouteRule = tableRouteRule;
        _tableMetadata=tableMetadata;
    }

    public string TableName => GetTableMetadata().LogicTableName;
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