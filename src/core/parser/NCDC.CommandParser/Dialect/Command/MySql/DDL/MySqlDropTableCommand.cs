using NCDC.CommandParser.Common.Command.DDL;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlDropTableCommand:DropTableCommand,IMySqlCommand
{
    public bool IfExists { get; }

    public MySqlDropTableCommand(bool ifExists)
    {
        IfExists = ifExists;
    }
}