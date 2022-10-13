using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlCreateTableCommand:CreateTableCommand,IMySqlCommand
{
    public bool IfNotExists { get; }

    public MySqlCreateTableCommand(bool ifNotExists,SimpleTableSegment table):base(table)
    {
        IfNotExists = ifNotExists;
    }
}