using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Dialect.Command.MySql;
using NCDC.CommandParser.Dialect.Command.MySql.DML;

namespace NCDC.CommandParser.Dialect.Handler.DML;

public sealed class SelectCommandHandler:ISqlCommandHandler
{
    public static LimitSegment? GetLimitSegment( SelectCommand selectCommand) {
        if (selectCommand is IMySqlCommand) {
            return ((MySqlSelectCommand) selectCommand).Limit;
        }
        // if (selectCommand instanceof PostgreSQLStatement) {
        //     return ((PostgreSQLSelectStatement) selectCommand).getLimit();
        // }
        // if (selectCommand instanceof SQL92Statement) {
        //     return ((SQL92SelectStatement) selectCommand).getLimit();
        // }
        // if (selectCommand instanceof SQLServerStatement) {
        //     return ((SQLServerSelectStatement) selectCommand).getLimit();
        // }
        // if (selectCommand instanceof OpenGaussStatement) {
        //     return ((OpenGaussSelectStatement) selectCommand).getLimit();
        // }
        return null;
    }
   
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