using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Dialect.Command.MySql;
using NCDC.CommandParser.Dialect.Command.MySql.DML;

namespace NCDC.CommandParser.Dialect.Handler.DML;

public sealed class SelectCommandHandler:ISelectCommandHandler
{
   
    public static LockSegment? GetLockSegment( SelectCommand selectCommand) {
        if (selectCommand is IMySqlCommand)
        {
            return ((MySqlSelectCommand)selectCommand).Lock;
        }
        //todo pgsql
        // if (selectCommand is PostgreSQLStatement) {
        //     return ((PostgreSQLSelectStatement) selectCommand).getLock();
        // }
        return null;
    } 
}