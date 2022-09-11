using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using OpenConnector.Exceptions;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

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