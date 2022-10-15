using NCDC.CommandParser.Common.Command.TCL;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Dialect.Command.MySql.TCL;

public sealed class MySqlLockCommand:LockCommand,IMySqlCommand
{
    public ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();

}