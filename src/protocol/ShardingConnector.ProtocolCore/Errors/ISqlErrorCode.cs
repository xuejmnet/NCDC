namespace ShardingConnector.ProtocolCore.Errors;

public interface ISqlErrorCode
{
    int GetErrorCode();
    string GetSqlState();
    string GetErrorMessage();
}