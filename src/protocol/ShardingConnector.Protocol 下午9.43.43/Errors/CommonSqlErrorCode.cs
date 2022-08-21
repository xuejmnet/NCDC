namespace ShardingConnector.Protocol.Errors;

public sealed class CommonSqlErrorCode
{
    private static readonly ISqlErrorCode _circuit_break_mode =new SqlErrorCode(1000, "C1000", "circuit break mode is on.");
    private static readonly ISqlErrorCode _scaling_job_not_exist =new SqlErrorCode(1201, "C1201", "scaling job {0} does not exist.");
    private static readonly ISqlErrorCode _scaling_operate_failed =new SqlErrorCode(1209, "C1209", "scaling operate failed: [{0}]");
    private static readonly ISqlErrorCode _database_write_locked =new SqlErrorCode(1300, "C1300", "the database {0} is read-only");
    private static readonly ISqlErrorCode _table_lock_wait_timeout =new SqlErrorCode(1301, "C1301", "the table {0} of schema {1} lock wait timeout of {2} ms exceeded");
    private static readonly ISqlErrorCode _table_locked =new SqlErrorCode(1302, "C1302", "the table {0} of schema %s is locked");
    private static readonly ISqlErrorCode _too_many_connections_exception =new SqlErrorCode(1040, "08004", "too many connections");
    private static readonly ISqlErrorCode _runtime_exception =new SqlErrorCode(1997, "C1997", "runtime exception: [{0}]");
    private static readonly ISqlErrorCode _unsupported_command =new SqlErrorCode(1998, "C1998", "unsupported command: [{0}]");
    private static readonly ISqlErrorCode _unknown_exception =new SqlErrorCode(1999, "C1999", "unknown exception:[{0}]");
    
    public static ISqlErrorCode CIRCUIT_BREAK_MODE { get; } =new SqlErrorCode(1000, "C1000", "circuit break mode is on.");
    public static ISqlErrorCode SCALING_JOB_NOT_EXIST_ARGS1 => _scaling_job_not_exist;
    public static ISqlErrorCode SCALING_OPERATE_FAILED_ARGS1 => _scaling_operate_failed;
    public static ISqlErrorCode DATABASE_WRITE_LOCKED_ARGS1 => _database_write_locked;
    public static ISqlErrorCode TABLE_LOCK_WAIT_TIMEOUT_ARGS3 => _table_lock_wait_timeout;
    public static ISqlErrorCode TABLE_LOCKED_ARGS1 => _table_locked;
    public static ISqlErrorCode TOO_MANY_CONNECTIONS_EXCEPTION => _too_many_connections_exception;
    public static ISqlErrorCode RUNTIME_EXCEPTION_ARGS1 => _runtime_exception;
    public static ISqlErrorCode UNSUPPORTED_COMMAND_ARGS1 => _unsupported_command;
    public static ISqlErrorCode UNKNOWN_EXCEPTION_ARGS1 => _unknown_exception;
}