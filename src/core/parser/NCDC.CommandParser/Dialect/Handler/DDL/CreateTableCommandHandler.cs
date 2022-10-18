using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Dialect.Command.MySql;
using NCDC.CommandParser.Dialect.Command.MySql.DDL;

namespace NCDC.CommandParser.Dialect.Handler.DDL;

public sealed class CreateTableCommandHandler:ISqlCommandHandler
{
    
    /**
     * Judge whether contains if not exists or not.
     *
     * @param createTableCommand create table statement
     * @return whether contains if not exists or not
     */
    public static bool IfNotExists( CreateTableCommand createTableCommand) {
        if (createTableCommand is IMySqlCommand)
        {
            return ((MySqlCreateTableCommand)createTableCommand).IfNotExists;
        }
        //todo pgsql
        // if (createTableCommand instanceof PostgreSQLStatement) {
        //     return ((PostgreSQLCreateTableStatement) createTableCommand).isIfNotExists();
        // }
        // if (createTableCommand instanceof OpenGaussStatement) {
        //     return ((OpenGaussCreateTableStatement) createTableCommand).isIfNotExists();
        // }
        return false;
    }
    
    /**
     * Get select statement.
     *
     * @param createTableCommand create table statement
     * @return select statement
     */
    public static SelectCommand? GetSelectCommand( CreateTableCommand createTableCommand) {
        //todo sqlserver
        // if (createTableCommand is SqlServerCommand) {
        //     return ((SQLServerCreateTableStatement) createTableCommand).getSelectStatement();
        // }
        return null;
    }
    
    /**
     * Get list of columns.
     *
     * @param createTableCommand create table statement
     * @return list of columns
     */
    public static ICollection<ColumnSegment> GetColumns( CreateTableCommand createTableCommand) {
        //todo sqlserver
        // if (createTableCommand is SqlServerCommand) {
        //     return ((SQLServerCreateTableStatement) createTableCommand).getColumns();
        // }
        return Array.Empty<ColumnSegment>();
    }
}