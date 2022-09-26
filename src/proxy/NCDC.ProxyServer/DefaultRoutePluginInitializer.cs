using System.Reflection;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyServer;

public sealed class DefaultRoutePluginInitializer:IRoutePluginInitializer
{
    private readonly IAppConfiguration _appConfiguration;

    public DefaultRoutePluginInitializer(IAppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }
    public Task InitializeAsync()
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
        return Task.CompletedTask;
    }
}