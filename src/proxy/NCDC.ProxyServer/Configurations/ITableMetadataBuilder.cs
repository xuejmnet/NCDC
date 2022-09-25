using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Configurations;

public interface ITableMetadataBuilder
{
    IReadOnlyDictionary<string, TableMetadata> Build(string databaseName);
}