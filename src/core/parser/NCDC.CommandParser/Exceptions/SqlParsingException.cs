using System.Runtime.Serialization;

namespace NCDC.CommandParser.Exceptions;

public class SqlParsingException:SqlParsingBaseException
{
    public SqlParsingException(string sql) : base($"You have an error in your SQL syntax: {sql}")
    {
    }

}