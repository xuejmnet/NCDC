using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlDropDatabaseCommand:DropDatabaseCommand,IMySqlCommand
{
    public MySqlDropDatabaseCommand(string databaseName, bool ifExists) : base(databaseName, ifExists)
    {
    }
}