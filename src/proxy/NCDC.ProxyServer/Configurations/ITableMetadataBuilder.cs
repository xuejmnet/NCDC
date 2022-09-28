using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Configurations;

public interface ITableMetadataBuilder
{
    Task<IReadOnlyList<TableMetadata>> BuildAsync(string databaseName);
}