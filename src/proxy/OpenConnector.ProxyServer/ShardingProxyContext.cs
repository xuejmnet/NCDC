using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.ProxyServer;

public sealed class ShardingProxyContext
{
    private static readonly ShardingProxyContext INSTANCE = new ShardingProxyContext();
    private   IDictionary<string, string> _properties = new Dictionary<string, string>();
    public Authentication Authentication { get; private set; }
    private bool _isCircuitBreak;
    private ShardingProxyContext(){}

    public static ShardingProxyContext GetInstance()
    {
        return INSTANCE;
    }

    public void Init(Authentication authentication, IDictionary<string, string> properties)
    {
        Authentication = authentication;
        _properties = properties;
    }
}