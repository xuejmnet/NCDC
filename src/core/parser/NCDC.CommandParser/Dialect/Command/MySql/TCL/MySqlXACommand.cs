using NCDC.CommandParser.Common.Command.TCL;

namespace NCDC.CommandParser.Dialect.Command.MySql.TCL;

public sealed class MySqlXACommand:XACommand,IMySqlCommand
{
    public MySqlXACommand(string op) : base(op)
    {
    }
}