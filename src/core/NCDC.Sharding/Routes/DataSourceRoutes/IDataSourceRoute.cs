using NCDC.CommandParserBinder.MetaData;
using NCDC.Sharding.Routes.Abstractions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}