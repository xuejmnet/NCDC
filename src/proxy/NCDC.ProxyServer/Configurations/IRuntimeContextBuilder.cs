using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextBuilder
{
    IRuntimeContext Build(string databaseName);
    // IRuntimeContext Build(ShardingConfigOption shardingConfigOption);
}