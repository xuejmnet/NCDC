// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using NCDC.Basic.TableMetadataManagers;
// using NCDC.EntityFrameworkCore.Entities;
// using NCDC.Extensions;
// using NCDC.ProxyServer.Abstractions;
// using NCDC.ProxyServer.Configurations;
// using NCDC.ProxyServer.Configurations.Initializers;
// using NCDC.ProxyServer.Contexts;
//
// namespace NCDC.EntityFrameworkCore.Configurations;
//
// public sealed class EntityFrameworkCoreAppInitializer:IAppInitializer
// {
//     private readonly IRuntimeContextLoader _runtimeContextLoader;
//     private readonly IAppConfiguration _appConfiguration;
//     private readonly IRuntimeContextBuilder _runtimeContextBuilder;
//     private readonly IServiceProvider _serviceProvider;
//
//     public EntityFrameworkCoreAppInitializer(IRuntimeContextLoader runtimeContextLoader,IAppConfiguration appConfiguration,IRuntimeContextBuilder runtimeContextBuilder,IServiceProvider serviceProvider)
//     {
//         _runtimeContextLoader = runtimeContextLoader;
//         _appConfiguration = appConfiguration;
//         _runtimeContextBuilder = runtimeContextBuilder;
//         _serviceProvider = serviceProvider;
//     }
//     public  async Task InitializeAsync()
//     {
//         
//         using (var scope = _serviceProvider.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
//             // var appUsers = await dbContext.Set<AppAuthUserEntity>().ToListAsync();
//             var logicDatabases = await dbContext.Set<LogicDatabaseEntity>().ToListAsync();
//             var dataSources = await dbContext.Set<DataSourceEntity>().ToListAsync();
//             // var logicTables = await dbContext.Set<LogicTableEntity>().ToListAsync();
//             // var actualTables = await dbContext.Set<ActualTableEntity>().ToListAsync();
//             // var logicDatabaseAuthUsers = await dbContext.Set<LogicDatabaseUserEntity>().ToListAsync();
//             foreach (var logicDatabase in logicDatabases)
//             {
//                 var builder = LogicDatabaseApplicationBuilder.CreateBuilder(logicDatabase.Name);
//                 builder.ConfigOption.AutoUseWriteConnectionStringAfterWriteDb =
//                     logicDatabase.AutoUseWriteConnectionStringAfterWriteDb;
//                 builder.ConfigOption.ThrowIfQueryRouteNotMatch = logicDatabase.ThrowIfQueryRouteNotMatch;
//                 builder.ConfigOption.MaxQueryConnectionsLimit = logicDatabase.MaxQueryConnectionsLimit;
//                 builder.ConfigOption.ConnectionMode = logicDatabase.ConnectionMode;
//                 foreach (var dataSourceEntity in dataSources)
//                 {
//                     if (dataSourceEntity.IsDefault)
//                     {
//                         builder.ConfigOption.AddDefaultDataSource(dataSourceEntity.Name,
//                             dataSourceEntity.ConnectionString);
//                     }
//                     else
//                     {
//                         builder.ConfigOption.AddExtraDataSource(dataSourceEntity.Name,
//                             dataSourceEntity.ConnectionString);
//                     }
//                 }
//
//                 var runtimeContext = builder.BuildAsync(_serviceProvider);
//                 //
//                 // var tables = logicTables.Where(o => o.Database == logicDatabase.Name).ToList();
//                 // foreach (var logicTable in tables)
//                 // {
//                 //     logicTable.Check();
//                 //     var tableMetadata = new TableMetadata(logicTable.LogicName,new Dictionary<string, ColumnMetadata>());
//                 //     var actualDataSourceTables = actualTables.Where(o=>o.LogicTableName==logicTable.LogicName&&o.Database==logicDatabase.Name).ToList();
//                 //     if (actualDataSourceTables.IsNotEmpty())
//                 //     {
//                 //         tableMetadata.TableNames
//                 //     }
//                 //
//                 //     if (logicTable.ShardingTableRule.NotNullOrWhiteSpace())
//                 //     {
//                 //         
//                 //     }
//                 // }
//
//                 _runtimeContextLoader.Load(runtimeContext);
//             }
//
//         }
//         //启动netty监听端口
//     }
// }