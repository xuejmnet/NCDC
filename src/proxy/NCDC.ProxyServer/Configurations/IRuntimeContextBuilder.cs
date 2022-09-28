using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextBuilder
{
    Task<IRuntimeContext> BuildAsync(string databaseName);
    // IRuntimeContext BuildAsync(ShardingConfigOption shardingConfigOption);
}