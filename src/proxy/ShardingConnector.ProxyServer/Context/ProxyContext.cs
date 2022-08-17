namespace ShardingConnector.ProxyServer.Context;

public sealed class ProxyContext
{
    public static ProxyContext Instance { get; } = new ProxyContext();
    public List<string> Databases = new List<string>(){"dbdbd0","dbdbd1","dbdbd2"};
}