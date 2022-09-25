using NCDC.Basic.TableMetadataManagers;
using NCDC.ProxyServer.Configurations;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbTableMetadataBuilder:ITableMetadataBuilder
{
    public IReadOnlyDictionary<string, TableMetadata> Build(string databaseName)
    {
        throw new NotImplementedException();
    }
}