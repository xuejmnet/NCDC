using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Routes.Abstractions;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}