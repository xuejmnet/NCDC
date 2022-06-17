namespace ShardingConnector.Proxy.Network;

public interface IShardingProxy
{
    Task StartAsync(CancellationToken cancellationToken=default);
    Task StopAsync(CancellationToken cancellationToken=default);
}