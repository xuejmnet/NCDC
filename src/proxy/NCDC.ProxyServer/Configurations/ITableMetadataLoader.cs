using NCDC.Basic.TableMetadataManagers;

namespace NCDC.ProxyServer.Configurations;

public interface ITableMetadataLoader
{
    IReadOnlyDictionary<string,TableMetadata> GetTableMetadatas(IReadOnlyCollection<IDataSourceConfig> dataSourceConfigs,IDictionary<string,List<(string logicTable,string actualTable)>> tableMetadataFromMap);
}