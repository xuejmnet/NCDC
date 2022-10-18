using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Dialect.Command.MySql;
using NCDC.CommandParser.Dialect.Command.MySql.DML;

namespace NCDC.CommandParser.Dialect.Handler.DML;

public sealed class InsertCommandHandler:ISqlCommandHandler
{
    
    /**
     * Get On duplicate key columns segment.
     *
     * @param insertCommand insert statement
     * @return on duplicate key columns segment
     */
    public static OnDuplicateKeyColumnsSegment? GetOnDuplicateKeyColumnsSegment( InsertCommand insertCommand) {
        if (insertCommand is IMySqlCommand) {
            return ((MySqlInsertCommand) insertCommand).OnDuplicateKeyColumns;
        }
        //todo pgsql
        // if (insertCommand is OpenGaussStatement) {
        //     return ((OpenGaussInsertCommand) insertCommand).getOnDuplicateKeyColumns();
        // }
        // if (insertCommand instanceof PostgreSQLStatement) {
        //     return ((PostgreSQLInsertCommand) insertCommand).getOnDuplicateKeyColumns();
        // }
        return null;
    }
    
    /**
     * Get set assignment segment.
     *
     * @param insertCommand insert statement
     * @return set assignment segment
     */
    public static SetAssignmentSegment? GetSetAssignmentSegment( InsertCommand insertCommand) {
        return insertCommand is IMySqlCommand ? ((MySqlInsertCommand) insertCommand).SetAssignment : null;
    }
    
    /**
     * Get with segment.
     *
     * @param insertCommand insert statement
     * @return with segment
     */
    public static WithSegment? GetWithSegment( InsertCommand insertCommand) {
        //todo pgsql
        // if (insertCommand is PostgreSQLStatement) {
        //     return ((PostgreSQLInsertCommand) insertCommand).getWithSegment();
        // }
        // if (insertCommand instanceof SQLServerStatement) {
        //     return ((SQLServerInsertCommand) insertCommand).getWithSegment();
        // }
        // if (insertCommand instanceof OpenGaussStatement) {
        //     return ((OpenGaussInsertCommand) insertCommand).getWithSegment();
        // }
        return null;
    }
    
    /**
     * Get output segment.
     * 
     * @param insertCommand insert statement
     * @return output segment
     */
    public static OutputSegment? GetOutputSegment( InsertCommand insertCommand) {
        //todo sqlserver
        // if (insertCommand is SQLServerStatement) {
        //     return ((SQLServerInsertCommand) insertCommand).getOutputSegment();
        // }
        return null;
    }
    
    /**
     * Get insert multi table element segment.
     *
     * @param insertCommand insert statement
     * @return insert multi table element segment
     */
    public static InsertMultiTableElementSegment? GetInsertMultiTableElementSegment( InsertCommand insertStatement) {
        // if (insertCommand instanceof OracleStatement) {
        //     return ((OracleInsertCommand) insertCommand).getInsertMultiTableElementSegment();
        // }
        // return Optional.empty();
        return null;
    }
    
    /**
     * Get select subquery segment.
     *
     * @param insertCommand insert statement
     * @return select subquery segment
     */
    public static SubQuerySegment? GetSelectSubQuery( InsertCommand insertCommand) {
        // if (insertCommand instanceof OracleStatement) {
        //     return ((OracleInsertCommand) insertCommand).getSelectSubquery();
        // }
        return null;
    }
}