using System.Reflection;
using NCDC.Helpers;
using NCDC.Plugin;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Contexts;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ProxyServer;

public sealed class DefaultAppInitializer:IAppInitializer
{
    private readonly IRuntimeContextLoader _runtimeContextLoader;
    private readonly IAppConfiguration _appConfiguration;
    private readonly IAppDatabaseConfiguration _appDatabaseConfiguration;
    private readonly IRuntimeContextBuilder _runtimeContextBuilder;
    private readonly IShardingConfigOptionBuilder _shardingConfigOptionBuilder;

    public DefaultAppInitializer(IRuntimeContextLoader runtimeContextLoader,IAppConfiguration appConfiguration,IAppDatabaseConfiguration appDatabaseConfiguration,IRuntimeContextBuilder runtimeContextBuilder,IShardingConfigOptionBuilder shardingConfigOptionBuilder)
    {
        _runtimeContextLoader = runtimeContextLoader;
        _appConfiguration = appConfiguration;
        _appDatabaseConfiguration = appDatabaseConfiguration;
        _runtimeContextBuilder = runtimeContextBuilder;
        _shardingConfigOptionBuilder = shardingConfigOptionBuilder;
    }
    public  async Task InitializeAsync()
    {
        var databases = _appDatabaseConfiguration.GetDatabases();
        foreach (var database in databases)
        {
            var runtimeContext =await  _runtimeContextBuilder.BuildAsync(database);
            _runtimeContextLoader.Load(runtimeContext);
        }
        //启动netty监听端口
    }
}