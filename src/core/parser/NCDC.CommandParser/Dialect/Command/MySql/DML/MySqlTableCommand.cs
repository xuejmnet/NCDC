using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlTableCommand:TableCommand,IMySqlCommand
{

    public SimpleTableSegment? Table { get; set; }
    public ColumnSegment? Column { get; set; }
    public LimitSegment? Limit { get; set; }
}