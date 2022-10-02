using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Contexts.Initializers;

public interface ITableMetadataInitializer
{
    Task InitializeAsync(TableMetadata tableMetadata);
}