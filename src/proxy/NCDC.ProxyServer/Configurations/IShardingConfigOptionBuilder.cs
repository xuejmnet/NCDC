using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Configurations;

public interface IShardingConfigOptionBuilder
{
    Task<ShardingConfigOption> BuildAsync(string databaseName);
}