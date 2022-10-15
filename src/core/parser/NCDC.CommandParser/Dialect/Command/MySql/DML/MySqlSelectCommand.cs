using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlSelectCommand:SelectCommand,IMySqlCommand
{
    public SimpleTableSegment? Table { get; set; }
    public LimitSegment? Limit { get; set; }
    public LockSegment? Lock { get; set; }
    public WindowSegment? Window { get; set; }
}