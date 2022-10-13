namespace NCDC.CommandParser.Common.Segment.Generic;

public enum PrivilegeTypeEnum
{
    SELECT,
    
    INSERT,
    
    UPDATE,
    
    DELETE,
    
    USAGE,
    
    CREATE,
    
    DROP,
    
    RELOAD,
    
    SHUTDOWN,
    
    PROCESS,
    
    FILE,
    
    GRANT,
    
    REFERENCES,
    
    INDEX,
    
    ALTER,
    
    SHOW_DB,
    
    SUPER,
    
    CREATE_TMP,
    
    LOCK_TABLES,
    
    EXECUTE,
    
    REPL_SLAVE,
    
    REPL_CLIENT,
    
    CREATE_VIEW,
    
    SHOW_VIEW,
    
    CREATE_PROC,
    
    ALTER_PROC,
    
    CREATE_USER,
    
    DROP_USER,
    
    EVENT,
    
    TRIGGER,
    
    CREATE_TABLESPACE,
    
    CREATE_ROLE,
    
    DROP_ROLE
}