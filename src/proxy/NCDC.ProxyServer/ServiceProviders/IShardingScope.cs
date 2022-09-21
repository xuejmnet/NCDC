namespace NCDC.ProxyServer.ServiceProviders;

public interface IShardingScope : IDisposable
{
    IShardingProvider ServiceProvider { get; }
}