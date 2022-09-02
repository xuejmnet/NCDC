namespace OpenConnector.CommandParser.Abstractions;

public interface ISqlCommandParser
{
    ISqlCommand Parse(string sql, bool useCache);
}