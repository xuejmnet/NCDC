// namespace NCDC.ProxyServer.Configurations;
//
// public interface ITableConfig
// {
//     string GetTableName();
//     /// <summary>
//     /// key: datasourceName
//     /// value: actualTableNames
//     /// </summary>
//     /// <returns></returns>
//     IDictionary<string,ISet<string>> GetActualTableNames();
//
//     (string datasource, string logicTable,string actualTable)? GetTableMetadataSchema();
//
//     string? GetDataSourceRouteRule();
//     string? GetShardingDataSourceColumn();
//     ISet<string> GetShardingDataSourceExtraColumnNames();
//     string? GetTableRouteRule();
//     string? GetShardingTableColumn();
//     ISet<string> GetShardingTableExtraColumnNames();
// }