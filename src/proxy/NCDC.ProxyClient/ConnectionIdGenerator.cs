namespace NCDC.ProxyClient;

public class ConnectionIdGenerator
{
    public static ConnectionIdGenerator Instance{get;}= new ConnectionIdGenerator();

    private ConnectionIdGenerator()
    {
        
    }

    private int _currentId;

    public int NextId()
    {
        var increment = Interlocked.Increment(ref _currentId);
        return increment;
    }
}