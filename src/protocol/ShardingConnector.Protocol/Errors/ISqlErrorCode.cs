namespace ShardingConnector.Protocol.Errors;

public interface ISqlErrorCode
{
    int GetErrorCode();
    string GetSqlState();
    string GetErrorMessage();
}