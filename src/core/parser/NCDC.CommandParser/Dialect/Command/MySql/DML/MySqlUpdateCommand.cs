using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Order;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlUpdateCommand:UpdateCommand,IMySqlCommand
{
    public OrderBySegment? OrderBy { get; set; }
    public LimitSegment? Limit { get; set; }
}