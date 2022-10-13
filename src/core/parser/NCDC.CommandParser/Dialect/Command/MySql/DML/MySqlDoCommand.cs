using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Dialect.Command.MySql.DML;

public sealed class MySqlDoCommand:DoCommand,IMySqlCommand
{
    public ICollection<IExpressionSegment>? Parameters { get; set; }
}