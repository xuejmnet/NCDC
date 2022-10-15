using NCDC.CommandParser.Common.Command.TCL;

namespace NCDC.CommandParser.Dialect.Command.MySql.TCL;

public sealed class MySqlSavepointCommand:SavepointCommand,IMySqlCommand
{
    public MySqlSavepointCommand(string savepointName) : base(savepointName)
    {
    }
}