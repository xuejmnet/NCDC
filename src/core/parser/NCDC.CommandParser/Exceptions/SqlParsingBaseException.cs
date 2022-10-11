namespace NCDC.CommandParser.Exceptions;

public class SqlParsingBaseException:Exception
{
    public SqlParsingBaseException(string? message) : base(message)
    {
    }
}