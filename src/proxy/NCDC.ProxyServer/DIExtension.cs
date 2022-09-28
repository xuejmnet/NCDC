using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.ServerDataReaders;
using NCDC.ProxyServer.ServerHandlers;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static IServiceCollection AddProxyServerCore(this IServiceCollection services)
    {
        services.AddSingleton<IAppUserInitializer, DefaultAppUserInitializer>();
        services.AddSingleton<IAppInitializer, DefaultAppInitializer>();
        services.AddSingleton<IRoutePluginInitializer, DefaultRoutePluginInitializer>();
        services.AddSingleton<IInitializerManager, DefaultInitializerManager>();
        services.AddSingleton<IContextManager, DefaultContextManager>();
        services.AddSingleton<IServerHandlerFactory, ServerHandlerFactory>();
        services.AddSingleton<IServerDataReaderFactory, ServerDataReaderFactory>();
        services.AddSingleton<IAppUserManager, DefaultAppUserManager>();
        return services;
    }
}