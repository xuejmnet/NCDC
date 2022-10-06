using NCDC.EntityFrameworkCore.Impls;

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
        services.AddAppService<EntityFrameworkCoreAppRuntimeBuilder,EntityFrameworkCoreAppInitializer>();
        return services;
    }
}