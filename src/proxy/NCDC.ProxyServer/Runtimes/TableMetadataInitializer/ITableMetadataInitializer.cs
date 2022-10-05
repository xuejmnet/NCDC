using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Runtimes.TableMetadataInitializer;

public interface ITableMetadataInitializer
{
    Task InitializeAsync(TableMetadata tableMetadata);
}