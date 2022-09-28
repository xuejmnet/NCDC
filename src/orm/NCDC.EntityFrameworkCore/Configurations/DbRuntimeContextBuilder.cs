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
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingParser;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbRuntimeContextBuilder:IRuntimeContextBuilder
{
    private readonly List<Action<IServiceCollection>> _serviceActions = new List<Action<IServiceCollection>>();
    private readonly string _databaseName;
    private readonly IShardingConfigOptionBuilder _shardingConfigOptionBuilder;
    private readonly ITableMetadataBuilder _tableMetadataBuilder;

    private Action<IShardingProvider, ShardingConfigOption> _shardingConfigOptionsConfigure;
    public DbRuntimeContextBuilder(string databaseName,IShardingConfigOptionBuilder shardingConfigOptionBuilder,ITableMetadataBuilder tableMetadataBuilder)
    {
        _databaseName = databaseName;
        _shardingConfigOptionBuilder = shardingConfigOptionBuilder;
        _tableMetadataBuilder = tableMetadataBuilder;
    }

    public IRuntimeContextBuilder UseConfig(Action<IShardingProvider, ShardingConfigOption> configure)
    {
        _shardingConfigOptionsConfigure = configure ?? throw new ArgumentNullException($"{nameof(configure)}");
        return this;
    }

    public IRuntimeContextBuilder AddServiceConfigure(Action<IServiceCollection> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        _serviceActions.Add(configure);;
        return this;
    }

    public  IRuntimeContext Build(IServiceProvider appServiceProvider)
    {
        var shardingRuntimeContext = new ShardingRuntimeContext(_databaseName);
        
        shardingRuntimeContext.Services.AddSingleton<IShardingProvider>(sp => new ShardingProvider(sp,appServiceProvider));
        var configOption = new ShardingConfigOption(_databaseName);
        shardingRuntimeContext.Services.AddSingleton<ShardingConfigOption>(sp =>
        {
            var shardingProvider = sp.GetRequiredService<IShardingProvider>();
            _shardingConfigOptionsConfigure?.Invoke(shardingProvider, configOption);
            configOption.CheckArguments();
            return configOption;
        });
        
        var shardingConfigOption = await _shardingConfigOptionBuilder.BuildAsync(databaseName);
   
       
        shardingRuntimeContext.Services.AddSingleton(shardingConfigOption);
        
        shardingRuntimeContext.Services.AddInternalRuntimeContextService();
        shardingRuntimeContext.Initialize();
        var tableMetadataManager = shardingRuntimeContext.GetTableMetadataManager();
        var tableMetadatas = await _tableMetadataBuilder.BuildAsync(databaseName);
        
        foreach (var tableMetadata in tableMetadatas)
        {
            tableMetadataManager.AddTableMetadata(tableMetadata);
        }

        return shardingRuntimeContext;
    }

}