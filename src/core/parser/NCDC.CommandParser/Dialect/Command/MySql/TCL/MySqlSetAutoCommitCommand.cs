using NCDC.CommandParser.Common.Command.TCL;

namespace NCDC.CommandParser.Dialect.Command.MySql.TCL;

public sealed class MySqlSetAutoCommitCommand:SetAutoCommitCommand,IMySqlCommand
{
    public MySqlSetAutoCommitCommand(bool autoCommit) : base(autoCommit)
    {
    }
}