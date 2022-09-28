using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.MySqlParser;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.DbProviderFactories;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.Options;
using NCDC.ShardingParser;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbRuntimeContextBuilder:IRuntimeContextBuilder
{
    private readonly IShardingConfigOptionBuilder _shardingConfigOptionBuilder;
    private readonly ITableMetadataBuilder _tableMetadataBuilder;

    public DbRuntimeContextBuilder(IShardingConfigOptionBuilder shardingConfigOptionBuilder,ITableMetadataBuilder tableMetadataBuilder)
    {
        _shardingConfigOptionBuilder = shardingConfigOptionBuilder;
        _tableMetadataBuilder = tableMetadataBuilder;
    }
    public async Task<IRuntimeContext> BuildAsync(string databaseName)
    {
        var shardingConfigOption = await _shardingConfigOptionBuilder.BuildAsync(databaseName);
   
        var shardingRuntimeContext = new ShardingRuntimeContext(databaseName);
       
        shardingRuntimeContext.Services.AddSingleton(shardingConfigOption);
        shardingRuntimeContext.Services.AddSingleton<IDbProviderFactory,ProxyDbProviderFactory>();
        shardingRuntimeContext.Services.AddSingleton<IVirtualDataSource,VirtualDataSource>();
        shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager,TableMetadataManager>();
        shardingRuntimeContext.Services.AddSingleton<IShardingExecutionContextFactory,ShardingExecutionContextFactory>();
        shardingRuntimeContext.Services.AddShardingParser();
        shardingRuntimeContext.Services.AddMySqlParser();
        shardingRuntimeContext.Services.AddShardingRoute();
        shardingRuntimeContext.Services.AddShardingRewrite();
        shardingRuntimeContext.Build();
        var tableMetadataManager = shardingRuntimeContext.GetTableMetadataManager();
        var tableMetadatas = await _tableMetadataBuilder.BuildAsync(databaseName);
        
        foreach (var tableMetadata in tableMetadatas)
        {
            tableMetadataManager.AddTableMetadata(tableMetadata);
        }

        return shardingRuntimeContext;
    }

}