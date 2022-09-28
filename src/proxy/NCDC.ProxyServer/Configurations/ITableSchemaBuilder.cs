using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Configurations;

public interface ITableSchemaBuilder
{
    Task<IDictionary<string /*logic table name*/, List<ColumnMetadata>>> BuildAsync(
        IDictionary<string /*logic table name*/, string /*actual table name*/> tables);
}