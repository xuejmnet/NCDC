using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingParser;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public interface IDataSourceRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}