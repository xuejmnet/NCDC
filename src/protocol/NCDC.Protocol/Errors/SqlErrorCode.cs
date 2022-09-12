namespace NCDC.Protocol.Errors;


public class SqlErrorCode:ISqlErrorCode
{
    private readonly int _errorCode;
    private readonly string _sqlState;
    private readonly string _errorMessage;

    public SqlErrorCode(int errorCode,string sqlState,string errorMessage)
    {
        _errorCode = errorCode;
        _sqlState = sqlState;
        _errorMessage = errorMessage;
    }
    public int GetErrorCode()
    {
        return _errorCode;
    }

    public string GetSqlState()
    {
        return _sqlState;
    }

    public string GetErrorMessage()
    {
        return _errorMessage;
    }
}