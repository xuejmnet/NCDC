using System.Reflection;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyServer.Configurations.Apps;

public class DefaultAppRulePluginLoader:IAppRulePluginLoader
{
    private readonly IAppConfiguration _appConfiguration;

    public DefaultAppRulePluginLoader(IAppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }
    public void Load()
    {
        
        //监听plugin目录
        var routePluginPath = _appConfiguration.GetRulePluginPath();
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
    }
}