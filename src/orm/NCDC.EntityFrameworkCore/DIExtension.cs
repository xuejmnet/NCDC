using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Impls;
using NCDC.Enums;
using NCDC.ProxyServer.AppServices.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    /// <summary>
    /// 添加efcore的配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEntityFrameworkCoreConfiguration(this IServiceCollection services)
    {
        //添加代理核心
        services.AddProxyServerCore();
        services.AddDbContextPool<NCDCDbContext>((sp, builder) =>
        {
            var appConfiguration = sp.GetRequiredService<IAppConfiguration>();
            if (DatabaseTypeEnum.MySql.Equals(appConfiguration.DatabaseType))
            {
                builder.UseMySql(appConfiguration.ConnectionsString, new MySqlServerVersion(new Version()));
                return;
            }

            throw new NotSupportedException($"{appConfiguration.DatabaseType}");
        });
        services.AddAppService<EntityFrameworkCoreAppRuntimeBuilder,EntityFrameworkCoreAppInitializer>();
        return services;
    }
}