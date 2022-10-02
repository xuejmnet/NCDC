using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Contexts.Initializers;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class EntityFrameworkCoreAppRuntimeContextBuilder : IAppRuntimeContextBuilder
{
    private readonly IServiceProvider _appServiceProvider;


    public EntityFrameworkCoreAppRuntimeContextBuilder(IServiceProvider appServiceProvider)
    {
        _appServiceProvider = appServiceProvider;
    }

    public async Task<IReadOnlyCollection<IRuntimeContext>> BuildAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabases = await dbContext.Set<LogicDatabaseEntity>().ToListAsync();
            var dataSources = await dbContext.Set<DataSourceEntity>().ToListAsync();
            var logicTables = await dbContext.Set<LogicTableEntity>().ToListAsync();
            var runtimeContexts = new List<IRuntimeContext>(logicDatabases.Count);
            foreach (var logicDatabase in logicDatabases)
            {
                var builder = LogicDatabaseApplicationBuilder.CreateBuilder(logicDatabase.Name);
                builder.ConfigOption.AutoUseWriteConnectionStringAfterWriteDb =
                    logicDatabase.AutoUseWriteConnectionStringAfterWriteDb;
                builder.ConfigOption.ThrowIfQueryRouteNotMatch = logicDatabase.ThrowIfQueryRouteNotMatch;
                builder.ConfigOption.MaxQueryConnectionsLimit = logicDatabase.MaxQueryConnectionsLimit;
                builder.ConfigOption.ConnectionMode = logicDatabase.ConnectionMode;
                foreach (var dataSourceEntity in dataSources)
                {
                    if (dataSourceEntity.IsDefault)
                    {
                        builder.ConfigOption.AddDefaultDataSource(dataSourceEntity.Name,
                            dataSourceEntity.ConnectionString);
                    }
                    else
                    {
                        builder.ConfigOption.AddExtraDataSource(dataSourceEntity.Name,
                            dataSourceEntity.ConnectionString);
                    }
                }

                var tables = logicTables.Where(o => o.Database == logicDatabase.Name).ToList();
                foreach (var logicTable in tables)
                {
                    logicTable.Check();
                    if (logicTable.ShardingTableRule != null)
                    {
                        builder.RouteConfigOption.AddTableRouteRule(logicTable.LogicName,logicTable.ShardingTableRule);
                    }
                    if (logicTable.ShardingDataSourceRule != null)
                    {
                        builder.RouteConfigOption.AddDataSourceRouteRule(logicTable.LogicName,logicTable.ShardingDataSourceRule);
                    }
                }
                builder.Services.AddSingleton<IRuntimeContextInitializer,EntityFrameworkCoreRuntimeContextInitializer>();
                var runtimeContext = await builder.BuildAsync(_appServiceProvider);
                runtimeContexts.Add(runtimeContext);
            }

            return runtimeContexts.AsReadOnly();
        }
    }
}