using NCDC.Basic.TableMetadataManagers;
using NCDC.ProxyServer.Configurations;

namespace NCDC.ProxyClientMySql.Configurations;

public class MySqlTableSchemaBuilder:ITableSchemaBuilder
{
    public async Task<IDictionary<string, List<ColumnMetadata>>> BuildAsync(IDictionary<string, string> tables)
    {
        throw new NotImplementedException();
    }
}