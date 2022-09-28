using Microsoft.Extensions.DependencyInjection;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;

namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextBuilder
{
    IRuntimeContextBuilder UseConfig(Action<IShardingProvider,ShardingConfigOption> configure);
    IRuntimeContextBuilder AddServiceConfigure(Action<IServiceCollection> configure);
    IRuntimeContext Build(IServiceProvider appServiceProvider);
    // IRuntimeContext BuildAsync(ShardingConfigOption shardingConfigOption);
}