using NCDC.Exceptions;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Configurations.Apps;

public class DefaultAppInitializer : IAppInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppRulePluginLoader _appRulePluginLoader;
    private readonly IAppRuntimeContextBuilder _appRuntimeContextBuilder;
    private readonly IAppRuntimeLoader _appRuntimeLoader;

    public DefaultAppInitializer(IServiceProvider serviceProvider, IAppRulePluginLoader appRulePluginLoader,
        IAppRuntimeContextBuilder appRuntimeContextBuilder, IAppRuntimeLoader appRuntimeLoader)
    {
        _serviceProvider = serviceProvider;
        _appRulePluginLoader = appRulePluginLoader;
        _appRuntimeContextBuilder = appRuntimeContextBuilder;
        _appRuntimeLoader = appRuntimeLoader;
    }

    public async Task InitializeAsync()
    {
        //加载路由规则插件
        _appRulePluginLoader.Load();
        //初始化运行时
        var runtimeContexts = await _appRuntimeContextBuilder.BuildAsync();
        foreach (var runtimeContext in runtimeContexts)
        {
            if (_appRuntimeLoader.HasLoaded(runtimeContext.DatabaseName))
            {
                throw new ShardingInvalidOperationException(
                    $"repeat load runtime context:[{runtimeContext.DatabaseName}]");
            }

            _appRuntimeLoader.Load(runtimeContext);
        }
    }
}