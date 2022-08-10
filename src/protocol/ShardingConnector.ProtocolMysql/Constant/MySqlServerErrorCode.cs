using ShardingConnector.ProtocolCore.Errors;

namespace ShardingConnector.ProtocolMysql.Constant;

public sealed class MySqlServerErrorCode
{
    public static ISqlErrorCode ER_DBACCESS_DENIED_ERROR_ARG3 { get; } =new SqlErrorCode(1044, "42000", "Access denied for user '{0}'@'{1}' to database '{2}'");
    public static ISqlErrorCode ER_ACCESS_DENIED_ERROR_ARG3 { get; } =new SqlErrorCode(1045, "28000", "Access denied for user '{0}'@'{1}' (using password: {2})");
    public static ISqlErrorCode ER_NO_DB_ERROR { get; } =new SqlErrorCode(1046, "3D000", "No database selected");
    public static ISqlErrorCode ER_BAD_DB_ERROR_ARG1 { get; } =new SqlErrorCode(1049, "42000", "Unknown database '{0}'");
    public static ISqlErrorCode ER_INTERNAL_ERROR_ARG1 { get; } =new SqlErrorCode(1815, "HY000", "Internal error: {0}");
    public static ISqlErrorCode ER_UNSUPPORTED_PS { get; } =new SqlErrorCode(1295, "HY000", "This command is not supported in the prepared statement protocol yet");
    public static ISqlErrorCode ER_DB_CREATE_EXISTS_ERROR_ARG1 { get; } =new SqlErrorCode(1007, "HY000", "Can't create database '{0}'; database exists");
    public static ISqlErrorCode ER_DB_DROP_NOT_EXISTS_ERROR_ARG1 { get; } =new SqlErrorCode(1008, "HY000", "Can't drop database '{0}'; database doesn't exist");
    public static ISqlErrorCode ER_TABLE_EXISTS_ERROR_ARG1 { get; } =new SqlErrorCode(1050, "42S01", "Table '{0}' already exists");
    public static ISqlErrorCode ER_NO_SUCH_TABLE_ARG1 { get; } =new SqlErrorCode(1146, "42S02", "Table '{0}' doesn't exist");
    public static ISqlErrorCode ER_NOT_SUPPORTED_YET_ARG1 { get; } =new SqlErrorCode(1235, "42000", "This version of ShardingSphere-Proxy doesn't yet support this SQL. '{0}'");
    public static ISqlErrorCode ER_SP_DOES_NOT_EXIST { get; } =new SqlErrorCode(1305, "42000", "Message: Data Source or ShardingSphere rule does not exist");
    public static ISqlErrorCode ER_CON_COUNT_ERROR { get; } =new SqlErrorCode(1040, "HY000", "Too many connections");
    public static ISqlErrorCode ER_UNKNOWN_CHARACTER_SET_ARG1 { get; } =new SqlErrorCode(1115, "42000", "Unknown character set: '{0}'");
    public static ISqlErrorCode ER_ERROR_ON_MODIFYING_GTID_EXECUTED_TABLE_ARG1 { get; } =new SqlErrorCode(3176, "HY000",
            "Please do not modify the {0} table with an XA transaction. This is an internal system table used to store GTIDs for committed transactions. "
                    + "Although modifying it can lead to an inconsistent GTID state, if necessary you can modify it with a non-XA transaction.");
}