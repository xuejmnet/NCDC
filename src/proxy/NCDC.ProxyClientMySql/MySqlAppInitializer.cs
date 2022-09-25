using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyClientMySql;

public sealed class MySqlAppInitializer:IAppInitializer
{
    private readonly IRuntimeContextLoader _runtimeContextLoader;
    private readonly IShardingConfiguration _shardingConfiguration;
    private readonly IRuntimeContextBuilder _runtimeContextBuilder;

    public MySqlAppInitializer(IRuntimeContextLoader runtimeContextLoader,IShardingConfiguration shardingConfiguration,IRuntimeContextBuilder runtimeContextBuilder)
    {
        _runtimeContextLoader = runtimeContextLoader;
        _shardingConfiguration = shardingConfiguration;
        _runtimeContextBuilder = runtimeContextBuilder;
    }
    public async Task InitializeAsync()
    {
        var appConfig = _shardingConfiguration.GetAppConfig();
        var configs = _shardingConfiguration.GetConfigs();
        foreach (var databaseConfig in configs)
        {
            var buildRuntimeContext = _runtimeContextBuilder.BuildRuntimeContext(databaseConfig);
            await _runtimeContextLoader.LoadAsync(buildRuntimeContext);
        }
    }
}