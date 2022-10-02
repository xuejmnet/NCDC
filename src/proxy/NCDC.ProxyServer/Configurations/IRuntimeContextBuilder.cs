using Microsoft.Extensions.DependencyInjection;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;

namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextBuilder
{
    Task<IReadOnlyCollection<IRuntimeContext>> BuildAsync(IServiceProvider appServiceProvider);
}