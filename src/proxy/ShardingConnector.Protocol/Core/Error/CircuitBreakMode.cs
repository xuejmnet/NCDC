namespace ShardingConnector.Protocol.Core.Error;

public class CircuitBreakMode:ISqlErrorCode
{
    public int GetErrorCode()
    {
        throw new NotImplementedException();
    }

    public string GetSqlState()
    {
        throw new NotImplementedException();
    }

    public string GetErrorMessage()
    {
        throw new NotImplementedException();
    }
}