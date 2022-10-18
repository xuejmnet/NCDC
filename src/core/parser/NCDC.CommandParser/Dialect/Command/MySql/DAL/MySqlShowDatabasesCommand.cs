using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DAL;
using NCDC.CommandParser.Common.Segment.DAL;

namespace NCDC.CommandParser.Dialect.Command.MySql.DAL;

public  sealed class MySqlShowDatabasesCommand:AbstractSqlCommand,IDALCommand
{
    public ShowFilterSegment? Filter { get; set; }
}