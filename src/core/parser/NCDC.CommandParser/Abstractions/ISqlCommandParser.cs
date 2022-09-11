namespace NCDC.CommandParser.Abstractions;

public interface ISqlCommandParser
{
    ISqlCommand Parse(string sql, bool useCache);
}