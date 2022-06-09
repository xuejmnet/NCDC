using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.Proxy.Common.Context;

public sealed class ShardingProxyContext
{
    private static readonly ShardingProxyContext INSTANCE = new ShardingProxyContext();
    private   IDictionary<string, string> _properties = new Dictionary<string, string>();
    private Authentication _authentication;
    private bool _isCircuitBreak;
    private ShardingProxyContext(){}

    public static ShardingProxyContext GetInstance()
    {
        return INSTANCE;
    }

    public void Init(Authentication authentication, IDictionary<string, string> properties)
    {
        _authentication = authentication;
        _properties = properties;
    }
}