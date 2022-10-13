using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlCreateDatabaseCommand:CreateDatabaseCommand,IMySqlCommand
{
    public MySqlCreateDatabaseCommand(string databaseName, bool ifNotExists) : base(databaseName, ifNotExists)
    {
    }
}