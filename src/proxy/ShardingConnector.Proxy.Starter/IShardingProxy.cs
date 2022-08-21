namespace ShardingConnector.Proxy.Starter;

public interface IShardingProxy
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}