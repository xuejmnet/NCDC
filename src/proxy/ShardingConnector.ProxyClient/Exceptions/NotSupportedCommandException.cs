namespace ShardingConnector.ProxyClient.Exceptions;

public class NotSupportedCommandException:ClientException
{
    private readonly string _commandType;

    public NotSupportedCommandException(string commandType)
    {
        _commandType = commandType;
    }
}