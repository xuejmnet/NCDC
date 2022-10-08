using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Runtimes.Initializer;

public interface ITableMetadataInitializer
{
    Task InitializeAsync(TableMetadata tableMetadata);
}