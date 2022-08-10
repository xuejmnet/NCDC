namespace ShardingConnector.ProxyClient.Core;

public class ConnectionIdGenerator
{
    private static readonly ConnectionIdGenerator _instance = new ConnectionIdGenerator();
    public static ConnectionIdGenerator GetInstance() => _instance;
    public ConnectionIdGenerator()
    {
        
    }

    private int _currentId;

    public int NextId()
    {
        var increment = Interlocked.Increment(ref _currentId);
        return increment;
    }
}