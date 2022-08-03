namespace ShardingConnector.ProtocolCore;

public class Check
{
    public static void ArgumentIfFail(bool expression, string error)
    {
        if (expression)
        {
            throw new ArgumentException(error);
        }
    }
    public static T ArgumentNotNull<T>(T argument, string error) where T:class
    {
        if (argument==null)
        {
            throw new ArgumentException(error);
        }

        return argument;
    }
}