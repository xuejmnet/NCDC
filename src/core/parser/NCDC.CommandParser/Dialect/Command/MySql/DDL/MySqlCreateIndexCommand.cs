using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DDL;

public sealed class MySqlCreateIndexCommand:CreateIndexCommand,IMySqlCommand
{
    public MySqlCreateIndexCommand(IndexSegment index, SimpleTableSegment table) : base(index, table)
    {
    }
}