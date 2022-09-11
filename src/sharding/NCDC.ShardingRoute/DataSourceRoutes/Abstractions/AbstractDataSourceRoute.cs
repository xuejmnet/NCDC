using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingParser;
using OpenConnector.Exceptions;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public abstract class AbstractDataSourceRoute:IDataSourceRoute
{
    private readonly TableMetadata _tableMetadata;
    protected AbstractDataSourceRoute(ITableMetadataManager tableMetadataManager)
    {
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

    public abstract ICollection<string> Route(SqlParserResult sqlParserResult);

}