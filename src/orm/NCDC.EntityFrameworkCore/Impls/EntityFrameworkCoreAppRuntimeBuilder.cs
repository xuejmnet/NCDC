using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Exceptions;
using NCDC.ProxyServer.AppServices.Builder;
using NCDC.ProxyServer.Contexts;

namespace NCDC.EntityFrameworkCore.Impls;

public class EntityFrameworkCoreAppRuntimeBuilder : IAppRuntimeBuilder
{
    private readonly IServiceProvider _appServiceProvider;

    public EntityFrameworkCoreAppRuntimeBuilder(IServiceProvider appServiceProvider)
    {
        _appServiceProvider = appServiceProvider;
    }

    public async Task<IRuntimeContext> BuildAsync(string database)
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabase = await dbContext.Set<LogicDatabaseEntity>().FirstOrDefaultAsync(o => o.Name == database);
            if (logicDatabase == null)
            {
                throw new ShardingConfigException($"database: {database} not found");
            }

            var dataSources = await dbContext.Set<DataSourceEntity>().Where(o => o.Database == database).ToListAsync();
            var logicTables = await dbContext.Set<LogicTableEntity>().Where(o => o.Database == database).ToListAsync();

            var builder = RuntimeApplicationBuilder.CreateBuilder(logicDatabase.Name);
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
                    builder.RouteConfigOption.AddTableRouteRule(logicTable.LogicName, logicTable.ShardingTableRule);
                }

                if (logicTable.ShardingDataSourceRule != null)
                {
                    builder.RouteConfigOption.AddDataSourceRouteRule(logicTable.LogicName,
                        logicTable.ShardingDataSourceRule);
                }
            }

            builder.AddRuntimeInitializer<EntityFrameworkCoreRuntimeInitializer>();
            return await builder.BuildAsync(_appServiceProvider);
        }
    }
}