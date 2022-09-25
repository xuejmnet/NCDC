using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.TableMetadataManagers;
using NCDC.MySqlParser;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Executors;
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
    public IRuntimeContext Build(string databaseName)
    {
        var shardingConfigOption = _shardingConfigOptionBuilder.Build(databaseName);
        var tableMetadataMap = _tableMetadataBuilder.Build(databaseName);
        var tableMetadataManager = new TableMetadataManager();
        IVirtualDataSource
        foreach (var tableName in tableMetadataMap.Keys)
        {
            tableMetadataManager.AddTableMetadata(tableMetadataMap[tableName]);
        }
        var shardingRuntimeContext = new ShardingRuntimeContext(shardingConfigOption.DatabaseName);
            
         shardingRuntimeContext.Services.AddSingleton<IVirtualDataSource>(logicDatabase);
         shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager>(sp=>tableMetadataManager);
         shardingRuntimeContext.Services.AddSingleton<IShardingExecutionContextFactory,TestShardingExecutionContextFactory>();
         shardingRuntimeContext.Services.AddShardingParser();
         shardingRuntimeContext.Services.AddMySqlParser();
         shardingRuntimeContext.Services.AddShardingRoute();
         shardingRuntimeContext.Services.AddShardingRewrite();
    }
}