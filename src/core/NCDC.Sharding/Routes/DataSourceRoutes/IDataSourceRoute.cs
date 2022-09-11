using NCDC.Basic.Parser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}