using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlDropIndexCommand:DropIndexCommand,IMySqlCommand
{
    public MySqlDropIndexCommand(SimpleTableSegment table)
    {
        Table = table;
    }

    public SimpleTableSegment Table { get; }
}