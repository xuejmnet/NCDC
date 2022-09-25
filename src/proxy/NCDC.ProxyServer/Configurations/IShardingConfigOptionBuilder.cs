using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Configurations;

public interface IShardingConfigOptionBuilder
{
    ShardingConfigOption Build(string databaseName);
}