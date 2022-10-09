using NCDC.Basic.Plugin;
using NCDC.Host;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;

namespace NCDC.ProxyServer.Bootstrappers;

public class AppBootstrapper:IAppBootstrapper
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IAppInitializer _appInitializer;
    private readonly IServiceHost _serviceHost;

    public AppBootstrapper(IAppConfiguration appConfiguration,IAppInitializer appInitializer,IServiceHost serviceHost)
    {
        _appConfiguration = appConfiguration;
        _appInitializer = appInitializer;
        _serviceHost = serviceHost;
    }
    public async Task StartAsync(CancellationToken cancellationToken=default)
    {
        InitRoutePlugin();
        await _appInitializer.InitializeAsync();
        await _serviceHost.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken=default)
    {
        return _serviceHost.StopAsync(cancellationToken);
    }

    private void InitRoutePlugin()
    {
        
        //监听plugin目录
        var routePluginPath = _appConfiguration.RulePluginPath;
        if (!Directory.Exists(routePluginPath))
        {
            Directory.CreateDirectory(routePluginPath);
        }
        PluginLoader.Init(routePluginPath);
      
        //获取当前文件夹下的所有dll进行load程序集加载获取所有的路由,将其存储到k-v内存中,
        var directoryInfo = new DirectoryInfo(routePluginPath);
        var files = directoryInfo.GetFiles().Where(o=>o.Name.EndsWith(".dll",StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var fileInfo in files)
        {
            PluginLoader.Instance.LoadFromAssemblyPath(fileInfo.FullName);
            // var loadFile = Assembly.LoadFile(fileInfo.FullName);
            // var type = loadFile.GetType("ShardingRoutePluginTest.TestModTableRouteRule");
        }
    }
}