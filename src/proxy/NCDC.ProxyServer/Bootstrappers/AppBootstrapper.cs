using NCDC.Basic.Plugin;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Configurations;

namespace NCDC.ProxyServer.Bootstrappers;

public class AppBootstrapper:IAppBootstrapper
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IAppInitializer _appInitializer;

    public AppBootstrapper(IAppConfiguration appConfiguration,IAppInitializer appInitializer)
    {
        _appConfiguration = appConfiguration;
        _appInitializer = appInitializer;
    }
    public async Task StartAsync()
    {
        InitRoutePlugin();
        await _appInitializer.InitializeAsync();
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