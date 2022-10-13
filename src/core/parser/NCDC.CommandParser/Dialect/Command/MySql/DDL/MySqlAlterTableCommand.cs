using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlAlterTableCommand:AlterTableCommand,IMySqlCommand
{
    public MySqlAlterTableCommand(SimpleTableSegment table) : base(table)
    {
    }
}