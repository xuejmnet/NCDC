using Microsoft.Extensions.DependencyInjection;
using NCDC.ProxyClient.Command;
using NCDC.ProxyClient.Command.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static IServiceCollection AddProxyClientCore(this IServiceCollection services)
    {
        services.AddSingleton<IMessageCommandProcessor, TaskMessageCommandProcessor>();
        services.AddSingleton<IMessageExecutorFactory, MessageExecutorFactory>();
        return services;
    }
}