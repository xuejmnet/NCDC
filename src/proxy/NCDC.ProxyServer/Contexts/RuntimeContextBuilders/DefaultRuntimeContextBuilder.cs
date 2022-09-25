// using System.Collections.Immutable;
// using Microsoft.Extensions.DependencyInjection;
// using MySqlConnector;
// using NCDC.Basic.Configurations;
// using NCDC.Basic.Metadatas;
// using NCDC.Basic.TableMetadataManagers;
// using NCDC.Enums;
// using NCDC.Exceptions;
// using NCDC.Extensions;
// using NCDC.Helpers;
// using NCDC.MySqlParser;
// using NCDC.ProxyServer.Abstractions;
// using NCDC.ProxyServer.Configurations;
// using NCDC.ProxyServer.Connection.Metadatas;
// using NCDC.ProxyServer.Executors;
// using NCDC.ShardingMerge;
// using NCDC.ShardingMerge.Abstractions;
// using NCDC.ShardingParser;
// using NCDC.ShardingRewrite;
// using NCDC.ShardingRoute;
// using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
// using NCDC.ShardingRoute.TableRoutes.Abstractions;
//
// namespace NCDC.ProxyServer.Contexts.RuntimeContextBuilders;
//
// public class DefaultRuntimeContextBuilder : IRuntimeContextBuilder
// {
//     private readonly IRouteConfiguration _routeConfiguration;
//     private readonly ITableMetadataLoader _tableMetadataLoader;
//     private readonly IDbProviderFactory _dbProviderFactory;
//
//     public DefaultRuntimeContextBuilder(IRouteConfiguration routeConfiguration,
//         ITableMetadataLoader tableMetadataLoader,IDbProviderFactory dbProviderFactory)
//     {
//         _routeConfiguration = routeConfiguration;
//         _tableMetadataLoader = tableMetadataLoader;
//         _dbProviderFactory = dbProviderFactory;
//     }
//
//     public IRuntimeContext BuildRuntimeContext(IDatabaseConfig databaseConfig)
//     {
//         var databaseName = databaseConfig.GetDatabaseName();
//         var shardingRuntimeContext = new ShardingRuntimeContext(databaseName);
//         var dataSourceConfigs = databaseConfig.GetDataSources();
//         var logicDatabase = new LogicDatabase(databaseName);
//         var shardingConfiguration = new ShardingConfiguration();
//         foreach (var dataSourceConfig in dataSourceConfigs)
//         {
//             //todo 抽象可能不是mysql
//             logicDatabase.AddDataSource(dataSourceConfig.GetDataSourceName(), dataSourceConfig.GetConnectionString(),
//                 _dbProviderFactory.Create(), dataSourceConfig.IsDefault());
//             if (dataSourceConfig.IsDefault())
//             {
//                 shardingConfiguration.AddDefaultDataSource(dataSourceConfig.GetDataSourceName(),
//                     dataSourceConfig.GetConnectionString());
//             }
//             else
//             {
//                 shardingConfiguration.AddExtraDataSource(dataSourceConfig.GetDataSourceName(),
//                     dataSourceConfig.GetConnectionString());
//             }
//         }
//
//         shardingRuntimeContext.Services.AddSingleton<ILogicDatabase>(logicDatabase);
//
//
//         shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager, TableMetadataManager>();
//         shardingRuntimeContext.Services.AddSingleton<IDataReaderMergerFactory, DataReaderMergerFactory>();
//         shardingRuntimeContext.Services.AddSingleton<IDatabaseSettings>(sp =>
//             new DatabaseSettings(databaseName, DatabaseTypeEnum.MySql));
//         shardingRuntimeContext.Services
//             .AddSingleton<IShardingExecutionContextFactory, ShardingExecutionContextFactory>();
//         shardingRuntimeContext.Services.AddShardingParser();
//         shardingRuntimeContext.Services.AddMySqlParser();
//         shardingRuntimeContext.Services.AddShardingRoute();
//         shardingRuntimeContext.Services.AddShardingRewrite();
//         shardingRuntimeContext.Services.AddSingleton(sp => shardingConfiguration);
//         shardingRuntimeContext.Build();
//         var tableMetadataManager = shardingRuntimeContext.GetTableMetadataManager();
//         var shardingTables = databaseConfig.GetShardingTables();
//         if (shardingTables.Any())
//         {
//             var tableMetadataFromMap = shardingTables.Where(o => o.GetTableMetadataSchema().HasValue)
//                 .Select(o => o.GetTableMetadataSchema()!.Value).GroupBy(o => o.datasource)
//                 .ToImmutableDictionary(o => o.Key, o => o.Select(g => (g.logicTable, g.actualTable)).ToList());
//             var tableMetadataDic =
//                 _tableMetadataLoader.GetTableMetadatas(databaseConfig.GetDataSources(), tableMetadataFromMap);
//             foreach (var shardingTable in shardingTables)
//             {
//                 var tableName = shardingTable.GetTableName();
//                 if (!tableMetadataDic.TryGetValue(tableName, out var tableMetadata))
//                 {
//                     continue;
//                 }
//
//                 bool isShardingDataSource = false;
//                 bool isShardingTable = false;
//                 var dataSourceRouteRule = shardingTable.GetDataSourceRouteRule();
//                 if (dataSourceRouteRule is not null)
//                 {
//                     isShardingDataSource = true;
//                     var shardingDataSourceColumn = shardingTable.GetShardingDataSourceColumn();
//                     if (shardingDataSourceColumn is null)
//                     {
//                         throw new ShardingConfigException(
//                             $"table:[{tableName}] sharding datasource not found sharding column");
//                     }
//
//                     tableMetadata.SetShardingDataSourceColumn(shardingDataSourceColumn);
//                     if (shardingTable.GetShardingDataSourceExtraColumnNames().IsNotEmpty())
//                     {
//                         foreach (var shardingDataSourceExtraColumn in shardingTable
//                                      .GetShardingDataSourceExtraColumnNames())
//                         {
//                             tableMetadata.AddExtraSharingDataSourceColumn(shardingDataSourceExtraColumn);
//                         }
//                     }
//                 }
//
//                 var tableRouteRule = shardingTable.GetTableRouteRule();
//                 if (tableRouteRule is not null)
//                 {
//                     isShardingTable = true;
//                     var shardingTableColumn = shardingTable.GetShardingTableColumn();
//                     if (shardingTableColumn is null)
//                     {
//                         throw new ShardingConfigException(
//                             $"table:[{tableName}] sharding table not found sharding column");
//                     }
//
//                     tableMetadata.SetShardingTableColumn(shardingTableColumn);
//                     if (shardingTable.GetShardingTableExtraColumnNames().IsNotEmpty())
//                     {
//                         foreach (var shardingTableExtraColumn in shardingTable.GetShardingTableExtraColumnNames())
//                         {
//                             tableMetadata.AddExtraSharingTableColumn(shardingTableExtraColumn);
//                         }
//                     }
//                     
//                 }
//
//                 foreach (var actualTableMapper in shardingTable.GetActualTableNames()
//                              .SelectMany(o => o.Value.Select(v => (o.Key, v))))
//                 {
//                     tableMetadata.AddActualTableWithDataSource(actualTableMapper.Key, actualTableMapper.v);
//                 }
//
//                 if (!isShardingDataSource && !isShardingTable)
//                 {
//                     throw new ShardingConfigException(
//                         $"table:[{tableName}] is not sharding datasource and is not sharding table");
//                 }
//
//                 tableMetadataManager.AddTableMetadata(tableMetadata);
//                 if (isShardingDataSource)
//                 {
//                     var dataSourceRouteRuleType = _routeConfiguration.GetRoute(dataSourceRouteRule!);
//                     var routeRule = shardingRuntimeContext.CreateInstance(dataSourceRouteRuleType);
//                     var shardingDataSourceRoute = shardingRuntimeContext.CreateInstance<ShardingDataSourceRoute>(routeRule);
//                     var dataSourceRouteManager = shardingRuntimeContext.GetDataSourceRouteManager();
//                     dataSourceRouteManager.AddRoute(shardingDataSourceRoute);
//                 }
//
//                 if (isShardingTable)
//                 {
//                     
//                     var tableRouteRuleType = _routeConfiguration.GetRoute(tableRouteRule!);
//                     var routeRule = shardingRuntimeContext.CreateInstance(tableRouteRuleType);
//                     var shardingTableRoute = shardingRuntimeContext.CreateInstance<ShardingTableRoute>(routeRule);
//                     var tableRouteManager = shardingRuntimeContext.GetTableRouteManager();
//                     tableRouteManager.AddRoute(shardingTableRoute);
//                 }
//             }
//         }
//     }
// }