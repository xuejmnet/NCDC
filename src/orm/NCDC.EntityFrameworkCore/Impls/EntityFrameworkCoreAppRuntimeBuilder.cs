using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Exceptions;
using NCDC.ProxyServer.AppServices.Builder;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Contexts;

namespace NCDC.EntityFrameworkCore.Impls;

public class EntityFrameworkCoreAppRuntimeBuilder : IAppRuntimeBuilder
{
    private readonly IServiceProvider _appServiceProvider;
    private readonly IAppConfiguration _appConfiguration;

    public EntityFrameworkCoreAppRuntimeBuilder(IServiceProvider appServiceProvider,IAppConfiguration appConfiguration)
    {
        _appServiceProvider = appServiceProvider;
        _appConfiguration = appConfiguration;
    }

    public async Task<IRuntimeContext> BuildAsync(string database)
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabase = await dbContext.Set<LogicDatabaseEntity>( ).FirstOrDefaultAsync(o => o.Name == database);
            if (logicDatabase == null)
            {
                throw new ShardingConfigException($"database: {database} not found");
            }

            var dataSources = await dbContext.Set<DataSourceEntity>().Where(o => o.Database == database).ToListAsync();
            var logicTables = await dbContext.Set<LogicTableEntity>().Where(o => o.Database == database).ToListAsync();

            var builder = RuntimeApplicationBuilder.CreateBuilder(_appConfiguration.GetDatabaseType(),logicDatabase.Name);
            builder.ConfigOption.AutoUseWriteConnectionStringAfterWriteDb =
                logicDatabase.AutoUseWriteConnectionStringAfterWriteDb;
            builder.ConfigOption.ThrowIfQueryRouteNotMatch = logicDatabase.ThrowIfQueryRouteNotMatch;
            builder.ConfigOption.MaxQueryConnectionsLimit = logicDatabase.MaxQueryConnectionsLimit;
            builder.ConfigOption.ConnectionMode = logicDatabase.ConnectionMode;
            foreach (var dataSource in dataSources)
            {
                if (dataSource.IsDefault)
                {
                    builder.ConfigOption.AddDefaultDataSource(dataSource.Name,
                        dataSource.ConnectionString);
                }
                else
                {
                    builder.ConfigOption.AddExtraDataSource(dataSource.Name,
                        dataSource.ConnectionString);
                }
            }

            foreach (var logicTable in logicTables)
            {
                logicTable.Check();
                if (logicTable.ShardingTableRule != null)
                {
                    builder.RouteConfigOption.AddTableRouteRule(logicTable.TableName, logicTable.ShardingTableRule);
                }

                if (logicTable.ShardingDataSourceRule != null)
                {
                    builder.RouteConfigOption.AddDataSourceRouteRule(logicTable.TableName,
                        logicTable.ShardingDataSourceRule);
                }
            }

            builder.AddRuntimeInitializer<EntityFrameworkCoreRuntimeInitializer>();
            return await builder.BuildAsync(_appServiceProvider);
        }
    }
}