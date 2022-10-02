using NCDC.EntityFrameworkCore.Configurations;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Contexts.Initializers;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    /// <summary>
    /// 添加efcore的存储
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEFCoreConfigurationStore(this IServiceCollection services)
    {
        //添加代理核心
        services.AddProxyServerCore();
        services.AddSingleton<IAppUserBuilder, EntityFrameworkCoreAppUserBuilder>();
        services.AddSingleton<IAppRuntimeContextBuilder, EntityFrameworkCoreAppRuntimeContextBuilder>();
        services.AddSingleton<IRuntimeContextInitializer, EntityFrameworkCoreRuntimeContextInitializer>();
        return services;
    }
}