// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using NCDC.Base;
// using NCDC.EntityFrameworkCore.Entities;
// using NCDC.ProxyServer.Configurations;
// using NCDC.ProxyServer.Connection.User;
// using NCDC.ProxyServer.Options;
//
// namespace NCDC.EntityFrameworkCore.Configurations;
//
// public sealed class DbShardingConfigOptionBuilder:IShardingConfigOptionBuilder
// {
//     private readonly IServiceProvider _serviceProvider;
//
//     public DbShardingConfigOptionBuilder(IServiceProvider serviceProvider)
//     {
//         _serviceProvider = serviceProvider;
//     }
//     public async Task<ShardingConfigOption> BuildAsync(string databaseName)
//     {
//         //todo 查询数据库获取databaseName的配置
//         using (var scope = _serviceProvider.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
//             var logicDatabase = await dbContext.Set<LogicDatabaseEntity>().Where(o=>o.Name==databaseName).FirstOrDefaultAsync();
//             var dataSources = await dbContext.Set<DataSourceEntity>().Where(o=>o.Name==databaseName).ToListAsync();
//             Check.NotNull(logicDatabase, databaseName);
//             
//             var shardingConfigOption = new ShardingConfigOption(databaseName);
//             shardingConfigOption.AutoUseWriteConnectionStringAfterWriteDb =logicDatabase!.AutoUseWriteConnectionStringAfterWriteDb;
//             shardingConfigOption.ThrowIfQueryRouteNotMatch =logicDatabase.ThrowIfQueryRouteNotMatch;
//             shardingConfigOption.MaxQueryConnectionsLimit =logicDatabase.MaxQueryConnectionsLimit;
//             shardingConfigOption.ConnectionMode =logicDatabase.ConnectionMode;
//             foreach (var dataSourceEntity in dataSources)
//             {
//                 if (dataSourceEntity.IsDefault)
//                 {
//                     shardingConfigOption.AddDefaultDataSource(dataSourceEntity.Name,dataSourceEntity.ConnectionString);
//                 }
//                 else
//                 {
//                     shardingConfigOption.AddExtraDataSource(dataSourceEntity.Name,dataSourceEntity.ConnectionString);  
//                 }
//             }
//
//             var users = await dbContext.Set<LogicDatabaseUserEntity>().Where(o=>o.Database==databaseName).ToListAsync();
//             var userNames = users.Select(o=>o.UserName).ToList();
//             var authUsers = await dbContext.Set<AppAuthUserEntity>().Where(o=>userNames.Contains(o.UserName)).ToListAsync();
//             foreach (var authUser in authUsers)
//             {
//                 shardingConfigOption.AddUser(new Grantee(authUser.UserName,authUser.Password));
//             }
//             shardingConfigOption.CheckArguments();
//             return shardingConfigOption;
//         }
//
//     }
// }