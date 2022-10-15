using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlInsertCommand:InsertCommand,IMySqlCommand
{
    public SetAssignmentSegment? SetAssignment { get; set; }
    public OnDuplicateKeyColumnsSegment? OnDuplicateKeyColumns { get; set; }
}