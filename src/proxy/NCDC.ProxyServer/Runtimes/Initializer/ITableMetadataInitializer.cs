using NCDC.Basic.Metadatas;

namespace NCDC.ProxyServer.Runtimes.Initializer;

public interface ITableMetadataInitializer
{
    Task<bool> InitializeAsync(TableMetadata tableMetadata);
}