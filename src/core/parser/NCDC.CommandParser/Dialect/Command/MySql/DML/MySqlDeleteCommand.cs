using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Order;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlDeleteCommand:DeleteCommand,IMySqlCommand
{
    public OrderBySegment? OrderBy { get; set; }
    public LimitSegment? Limit { get; set; }
    public MySqlDeleteCommand(SimpleTableSegment table) : base(table)
    {
    }
}