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
    private readonly IRouteConfiguration _routeConfiguration;
    private readonly IShardingConfiguration _shardingConfiguration;
    private readonly IRuntimeContextBuilder _runtimeContextBuilder;

    public DefaultAppInitializer(IRuntimeContextLoader runtimeContextLoader,IAppConfiguration appConfiguration,IRouteConfiguration routeConfiguration,IShardingConfiguration shardingConfiguration,IRuntimeContextBuilder runtimeContextBuilder)
    {
        _runtimeContextLoader = runtimeContextLoader;
        _appConfiguration = appConfiguration;
        _routeConfiguration = routeConfiguration;
        _shardingConfiguration = shardingConfiguration;
        _runtimeContextBuilder = runtimeContextBuilder;
    }
    public async Task InitializeAsync()
    {
        //监听plugin目录
        var routePluginPath = _appConfiguration.GetRoutePluginPath();
        if (!Directory.Exists(routePluginPath))
        {
            Directory.CreateDirectory(routePluginPath);
        }

        //获取当前文件夹下的所有dll进行load程序集加载获取所有的路由,将其存储到k-v内存中,
        var directoryInfo = new DirectoryInfo(routePluginPath);
        var files = directoryInfo.GetFiles().Where(o=>o.Name.EndsWith(".dll",StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var fileInfo in files)
        {
            Assembly.LoadFile(fileInfo.FullName);
        }
        if (_routeConfiguration is IDynamicRouteConfiguration dynamicRouteConfiguration)
        {
            foreach (var routeType in RuntimeHelper.GetImplementTypes(typeof(IRoute)))
            {
                dynamicRouteConfiguration.AddRoute(routeType);
            }
        }
        //设置
        var configs = _shardingConfiguration.GetConfigs();
        foreach (var databaseConfig in configs)
        {
            var buildRuntimeContext = _runtimeContextBuilder.BuildRuntimeContext(databaseConfig);
             _runtimeContextLoader.Load(buildRuntimeContext);
        }
    }
}