namespace NCDC.ProxyServer.Configurations;

public interface IShardingConfiguration
{
    IReadOnlyList<IDatabaseConfig> GetConfigs();
}