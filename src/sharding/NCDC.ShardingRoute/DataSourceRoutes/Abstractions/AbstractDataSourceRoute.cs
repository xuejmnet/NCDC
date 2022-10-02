using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public abstract class AbstractDataSourceRoute:IDataSourceRoute
{
    private readonly IDataSourceRouteRule _dataSourceRouteRule;
    private readonly TableMetadata _tableMetadata;
    protected AbstractDataSourceRoute(IDataSourceRouteRule dataSourceRouteRule,TableMetadata tableMetadata)
    {
        _dataSourceRouteRule = dataSourceRouteRule;
        _tableMetadata = tableMetadata;
    }

    public  string TableName => GetTableMetadata().LogicTableName;
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