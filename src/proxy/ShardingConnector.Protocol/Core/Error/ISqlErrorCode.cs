namespace ShardingConnector.Protocol.Core.Error;

public interface ISqlErrorCode
{
    int GetErrorCode();
    string GetSqlState();
    string GetErrorMessage();
}