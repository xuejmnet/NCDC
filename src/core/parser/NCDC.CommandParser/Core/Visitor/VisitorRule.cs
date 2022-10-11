using Antlr4.Runtime.Tree;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Exceptions;

namespace NCDC.CommandParser.Core.Visitor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 10:29:40
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class VisitorRule
    {
        private static readonly IDictionary<VisitorRuleEnum, VisitorRule> _rules = new Dictionary<VisitorRuleEnum, VisitorRule>();
        private static readonly IDictionary<string, VisitorRuleEnum> _ruleNameIndexs = new Dictionary<string, VisitorRuleEnum>();
        public SqlCommandTypeEnum SqlCommandType { get; }
        private readonly string _name;

        private string ContextName => _name + "Context";

        static VisitorRule()
        {
             AddRule(VisitorRuleEnum.SELECT,new VisitorRule("Select", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.TABLE,new VisitorRule("Table", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.INSERT,new VisitorRule("Insert", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.UPDATE,new VisitorRule("Update", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.DELETE,new VisitorRule("Delete", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.MERGE,new VisitorRule("Merge", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.REPLACE,new VisitorRule("Replace", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.COPY,new VisitorRule("Copy", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.LOCKTABLE,new VisitorRule("LockTable", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.CREATE_TABLE,new VisitorRule("CreateTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_AGGREGATE,new VisitorRule("CreateAggregate", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.RENAME_TABLE,new VisitorRule("RenameTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TABLE,new VisitorRule("AlterTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TYPE,new VisitorRule("AlterType", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_AGGREGATE,new VisitorRule("AlterAggregate", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_COLLATION,new VisitorRule("AlterCollation", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DEFAULT_PRIVILEGES,new VisitorRule("AlterDefaultPrivileges", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_FOREIGN_DATA_WRAPPER,new VisitorRule("AlterForeignDataWrapper", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_FOREIGN_TABLE,new VisitorRule("AlterForeignTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_FOREIGN_TABLE,new VisitorRule("DropForeignTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_GROUP,new VisitorRule("AlterGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_MATERIALIZED_VIEW,new VisitorRule("AlterMaterializedView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_MATERIALIZED_VIEW_LOG,new VisitorRule("AlterMaterializedViewLog", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_PLUGGABLE_DATABASE,new VisitorRule("AlterPluggableDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_OPERATOR,new VisitorRule("AlterOperator", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_TABLE,new VisitorRule("DropTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.TRUNCATE_TABLE,new VisitorRule("TruncateTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_INDEX,new VisitorRule("CreateIndex", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_INDEX,new VisitorRule("AlterIndex", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_INDEX,new VisitorRule("DropIndex", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_PROCEDURE,new VisitorRule("CreateProcedure", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_PUBLICATION,new VisitorRule("CreatePublication", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_PUBLICATION,new VisitorRule("AlterPublication", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SUBSCRIPTION,new VisitorRule("AlterSubscription", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_PROCEDURE,new VisitorRule("AlterProcedure", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_STATEMENT,new VisitorRule("AlterStatement", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_PROCEDURE,new VisitorRule("DropProcedure", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_ROUTINE,new VisitorRule("DropRoutine", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_RULE,new VisitorRule("DropRule", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_STATISTICS,new VisitorRule("DropStatistics", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_PUBLICATION,new VisitorRule("DropPublication", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SUBSCRIPTION,new VisitorRule("DropSubscription", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_FUNCTION,new VisitorRule("CreateFunction", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_FUNCTION,new VisitorRule("AlterFunction", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_CAST,new VisitorRule("DropCast", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_FUNCTION,new VisitorRule("DropFunction", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_GROUP,new VisitorRule("DropGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DATABASE,new VisitorRule("CreateDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DATABASE_LINK,new VisitorRule("CreateDatabaseLink", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DATABASE,new VisitorRule("AlterDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DATABASE_LINK,new VisitorRule("AlterDatabaseLink", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DATABASE,new VisitorRule("DropDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DATABASE_LINK,new VisitorRule("DropDatabaseLink", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DATABASE_DICTIONARY,new VisitorRule("AlterDatabaseDictionary", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DIMENSION,new VisitorRule("CreateDimension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DIMENSION,new VisitorRule("AlterDimension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DIMENSION,new VisitorRule("DropDimension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DIRECTORY,new VisitorRule("AlterDirectory", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DIRECTORY,new VisitorRule("DropDirectory", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_EVENT,new VisitorRule("CreateEvent", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_EDITION,new VisitorRule("CreateEdition", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_EVENT,new VisitorRule("AlterEvent", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_EVENT,new VisitorRule("DropEvent", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_INSTANCE,new VisitorRule("AlterInstance", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_LOGFILE_GROUP,new VisitorRule("CreateLogfileGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_LOGFILE_GROUP,new VisitorRule("AlterLogfileGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_LOGFILE_GROUP,new VisitorRule("DropLogfileGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_SERVER,new VisitorRule("CreateServer", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_SYNONYM,new VisitorRule("CreateSynonym", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SYNONYM,new VisitorRule("DropSynonym", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DIRECTORY,new VisitorRule("CreateDirectory", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SERVER,new VisitorRule("AlterServer", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_STATISTICS,new VisitorRule("AlterStatistics", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SESSION,new VisitorRule("AlterSession", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SYSTEM,new VisitorRule("AlterSystem", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SERVER,new VisitorRule("DropServer", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_TRIGGER,new VisitorRule("CreateTrigger", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TRIGGER,new VisitorRule("AlterTrigger", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_TRIGGER,new VisitorRule("DropTrigger", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_EVENT_TRIGGER,new VisitorRule("DropEventTrigger", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_VIEW,new VisitorRule("CreateView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_VIEW,new VisitorRule("AlterView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_PACKAGE,new VisitorRule("DropPackage", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_PACKAGE,new VisitorRule("AlterPackage", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_VIEW,new VisitorRule("DropView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ANALYZE,new VisitorRule("Analyze", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_SEQUENCE,new VisitorRule("CreateSequence", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SEQUENCE,new VisitorRule("AlterSequence", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SEQUENCE,new VisitorRule("DropSequence", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SYNONYM,new VisitorRule("AlterSynonym", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.PREPARE,new VisitorRule("Prepare", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.EXECUTE_STMT,new VisitorRule("ExecuteStmt", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DEALLOCATE,new VisitorRule("Deallocate", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_TABLESPACE,new VisitorRule("CreateTablespace", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TABLESPACE,new VisitorRule("AlterTablespace", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_TABLESPACE,new VisitorRule("DropTablespace", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_TEXT_SEARCH,new VisitorRule("DropTextSearch", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ASSOCIATE_STATISTICS,new VisitorRule("AssociateStatistics", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DISASSOCIATE_STATISTICS,new VisitorRule("DisassociateStatistics", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.AUDIT,new VisitorRule("Audit", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.NOAUDIT,new VisitorRule("NoAudit", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.COMMENT,new VisitorRule("Comment", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.FLASHBACK_DATABASE,new VisitorRule("FlashbackDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.FLASHBACK_TABLE,new VisitorRule("FlashbackTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.PURGE,new VisitorRule("Purge", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.RENAME,new VisitorRule("Rename", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_ROUTINE,new VisitorRule("AlterRoutine", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_EXTENSION,new VisitorRule("CreateExtension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_EXTENSION,new VisitorRule("AlterExtension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_EXTENSION,new VisitorRule("DropExtension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_RULE,new VisitorRule("AlterRule", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DECLARE,new VisitorRule("Declare", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DISCARD,new VisitorRule("Discard", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.LISTEN,new VisitorRule("Listen", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.NOTIFY,new VisitorRule("NotifyStmt", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.REFRESH_MATERIALIZED_VIEW,new VisitorRule("RefreshMatViewStmt", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.REINDEX,new VisitorRule("Reindex", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.SECURITY_LABEL,new VisitorRule("SecurityLabelStmt", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.UNLISTEN,new VisitorRule("Unlisten", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.SET_CONSTRAINTS,new VisitorRule("SetConstraints", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.SET_TRANSACTION,new VisitorRule("SetTransaction", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.SET_IMPLICIT_TRANSACTIONS,new VisitorRule("SetImplicitTransactions", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.BEGIN_TRANSACTION,new VisitorRule("BeginTransaction", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.BEGIN_DISTRIBUTED_TRANSACTION,new VisitorRule("BeginDistributedTransaction", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.START_TRANSACTION,new VisitorRule("StartTransaction", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.END,new VisitorRule("End", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.SET_AUTOCOMMIT,new VisitorRule("SetAutoCommit", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.COMMIT,new VisitorRule("Commit", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.COMMIT_WORK,new VisitorRule("CommitWork", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.ROLLBACK,new VisitorRule("Rollback", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.ROLLBACK_WORK,new VisitorRule("RollbackWork", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.SAVEPOINT,new VisitorRule("Savepoint", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.RELEASE_SAVEPOINT,new VisitorRule("ReleaseSavepoint", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.ROLLBACK_TO_SAVEPOINT,new VisitorRule("RollbackToSavepoint", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.LOCK,new VisitorRule("Lock", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.UNLOCK,new VisitorRule("Unlock", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.COMMIT_PREPARED,new VisitorRule("CommitPrepared", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.ROLLBACK_PREPARED,new VisitorRule("RollbackPrepared", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.GRANT,new VisitorRule("Grant", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.GRANT_ROLE_OR_PRIVILEGE_TO,new VisitorRule("GrantRoleOrPrivilegeTo", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.GRANT_ROLE_OR_PRIVILEGE_ON_TO,new VisitorRule("GrantRoleOrPrivilegeOnTo", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.GRANT_PROXY,new VisitorRule("GrantPROXY", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.REVOKE,new VisitorRule("Revoke", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.REVOKE_FROM,new VisitorRule("RevokeFrom", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.REVOKE_ON_FROM,new VisitorRule("RevokeOnFrom", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.CREATE_USER,new VisitorRule("CreateUser", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.ALTER_USER,new VisitorRule("AlterUser", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.DROP_USER,new VisitorRule("DropUser", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.DENY_USER,new VisitorRule("Deny", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.RENAME_USER,new VisitorRule("RenameUser", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.SET_USER,new VisitorRule("SetUser", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.CREATE_ROLE,new VisitorRule("CreateRole", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.ALTER_ROLE,new VisitorRule("AlterRole", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.DROP_ROLE,new VisitorRule("DropRole", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.CREATE_LOGIN,new VisitorRule("CreateLogin", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.ALTER_LOGIN,new VisitorRule("AlterLogin", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.DROP_LOGIN,new VisitorRule("DropLogin", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.SET_DEFAULT_ROLE,new VisitorRule("SetDefaultRole", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.SET_ROLE,new VisitorRule("SetRole", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.SET_PASSWORD,new VisitorRule("SetPassword", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.REVERT,new VisitorRule("Revert", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.USE,new VisitorRule("Use", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.DESC,new VisitorRule("Desc", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.HELP,new VisitorRule("Help", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.EXPLAIN,new VisitorRule("Explain", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_DATABASES,new VisitorRule("ShowDatabases", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_TABLES,new VisitorRule("ShowTables", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_EVENTS,new VisitorRule("ShowEvents", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_CHARACTER_SET,new VisitorRule("ShowCharacterSet", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_COLLATION,new VisitorRule("ShowCollation", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_VARIABLES,new VisitorRule("ShowVariables", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_TABLE_STATUS,new VisitorRule("ShowTableStatus", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_COLUMNS,new VisitorRule("ShowColumns", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_INDEX,new VisitorRule("ShowIndex", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_CREATE_TABLE,new VisitorRule("ShowCreateTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_OTHER,new VisitorRule("ShowOther", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_REPLICAS,new VisitorRule("ShowReplicas", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_REPLICA_STATUS,new VisitorRule("ShowReplicaStatus", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_SLAVE_HOSTS,new VisitorRule("ShowSlaveHosts", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_SLAVE_STATUS,new VisitorRule("ShowSlaveStatus", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_STATUS,new VisitorRule("ShowStatus", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW,new VisitorRule("Show", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_RELAYLOG_EVENTS,new VisitorRule("ShowRelaylogEventsStatement", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_PROCEDURE_CODE,new VisitorRule("ShowProcedureCodeStatement", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_OPEN_TABLES,new VisitorRule("ShowOpenTables", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHOW_TRIGGERS,new VisitorRule("ShowTriggers", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SET_VARIABLE,new VisitorRule("SetVariable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SET,new VisitorRule("Set", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SET_NAME,new VisitorRule("SetName", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SET_CHARACTER,new VisitorRule("SetCharacter", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.RESET_PARAMETER,new VisitorRule("ResetParameter", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.VACUUM,new VisitorRule("Vacuum", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CREATE_LOADABLE_FUNCTION,new VisitorRule("CreateLoadableFunction", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.ANALYZE_TABLE,new VisitorRule("AnalyzeTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.LOAD,new VisitorRule("Load", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.INSTALL,new VisitorRule("Install", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.UNINSTALL,new VisitorRule("Uninstall", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.FLUSH,new VisitorRule("Flush", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.RESTART,new VisitorRule("Restart", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SHUTDOWN,new VisitorRule("Shutdown", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CREATE_RESOURCE_GROUP,new VisitorRule("CreateResourceGroup", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.ALTER_RESOURCE_COST,new VisitorRule("AlterResourceCost", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.SET_RESOURCE_GROUP,new VisitorRule("SetResourceGroup", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.BINLOG,new VisitorRule("Binlog", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.OPTIMIZE_TABLE,new VisitorRule("OptimizeTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CLONE,new VisitorRule("Clone", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.REPAIR_TABLE,new VisitorRule("RepairTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.KILL,new VisitorRule("Kill", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.RESET,new VisitorRule("ResetStatement", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.RESET_PERSIST,new VisitorRule("ResetPersistStatement", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CACHE_INDEX,new VisitorRule("CacheIndex", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.LOAD_INDEX_INFO,new VisitorRule("LoadIndexInfo", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CHECK_TABLE,new VisitorRule("CheckTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CHECKSUM_TABLE,new VisitorRule("ChecksumTable", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.DROP_RESOURCE_GROUP,new VisitorRule("DropResourceGroup", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.ALTER_RESOURCE_GROUP,new VisitorRule("AlterResourceGroup", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.DELIMITER,new VisitorRule("Delimiter", SqlCommandTypeEnum.DAL));
             AddRule(VisitorRuleEnum.CALL,new VisitorRule("Call", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.CHANGE_MASTER,new VisitorRule("ChangeMaster", SqlCommandTypeEnum.RL));
             AddRule(VisitorRuleEnum.START_SLAVE,new VisitorRule("StartSlave", SqlCommandTypeEnum.RL));
             AddRule(VisitorRuleEnum.STOP_SLAVE,new VisitorRule("StopSlave", SqlCommandTypeEnum.RL));
             AddRule(VisitorRuleEnum.XA,new VisitorRule("Xa", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.ABORT,new VisitorRule("Abort", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.CREATE_SCHEMA,new VisitorRule("CreateSchema", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SCHEMA,new VisitorRule("AlterSchema", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SCHEMA,new VisitorRule("DropSchema", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_SERVICE,new VisitorRule("CreateService", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_SERVICE,new VisitorRule("AlterService", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_SERVICE,new VisitorRule("DropService", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DOMAIN,new VisitorRule("DropDomain", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DOMAIN,new VisitorRule("CreateDomain", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_RULE,new VisitorRule("CreateRule", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_LANGUAGE,new VisitorRule("CreateLanguage", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_LANGUAGE,new VisitorRule("AlterLanguage", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_LANGUAGE,new VisitorRule("DropLanguage", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_CONVERSION,new VisitorRule("CreateConversion", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_CAST,new VisitorRule("CreateCast", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_TYPE,new VisitorRule("CreateType", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_CONVERSION,new VisitorRule("DropConversion", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DOMAIN,new VisitorRule("AlterDomain", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_POLICY,new VisitorRule("AlterPolicy", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_CONVERSION,new VisitorRule("AlterConversion", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_TEXT_SEARCH,new VisitorRule("CreateTextSearch", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TEXT_SEARCH_CONFIGURATION,new VisitorRule("AlterTextSearchConfiguration", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TEXT_SEARCH_DICTIONARY,new VisitorRule("AlterTextSearchDictionary", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TEXT_SEARCH_TEMPLATE,new VisitorRule("AlterTextSearchTemplate", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_TEXT_SEARCH_PARSER,new VisitorRule("AlterTextSearchParser", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_POLICY,new VisitorRule("DropPolicy", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_OWNED,new VisitorRule("DropOwned", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_OPERATOR,new VisitorRule("DropOperator", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_MATERIALIZED_VIEW,new VisitorRule("DropMaterializedView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_AGGREGATE,new VisitorRule("DropAggregate", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_COLLATION,new VisitorRule("DropCollation", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_FOREIGN_DATA_WRAPPER,new VisitorRule("DropForeignDataWrapper", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_TYPE,new VisitorRule("DropType", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_OPERATOR_CLASS,new VisitorRule("DropOperatorClass", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_OPERATOR_FAMILY,new VisitorRule("DropOperatorFamily", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_ACCESS_METHOD,new VisitorRule("DropAccessMethod", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_OUTLINE,new VisitorRule("DropOutline", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_OUTLINE,new VisitorRule("AlterOutline", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_ANALYTIC_VIEW,new VisitorRule("AlterAnalyticView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_EDITION,new VisitorRule("DropEdition", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_ATTRIBUTE_DIMENSION,new VisitorRule("AlterAttributeDimension", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_CONTEXT,new VisitorRule("CreateContext", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_SPFILE,new VisitorRule("CreateSPFile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_PFILE,new VisitorRule("CreatePFile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_CONTROL_FILE,new VisitorRule("CreateControlFile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_FLASHBACK_ARCHIVE,new VisitorRule("CreateFlashbackArchive", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_FLASHBACK_ARCHIVE,new VisitorRule("AlterFlashbackArchive", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_FLASHBACK_ARCHIVE,new VisitorRule("DropFlashbackArchive", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_DISKGROUP,new VisitorRule("CreateDiskgroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_DISKGROUP,new VisitorRule("DropDiskgroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_ROLLBACK_SEGMENT,new VisitorRule("CreateRollbackSegment", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_ROLLBACK_SEGMENT,new VisitorRule("DropRollbackSegment", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_LOCKDOWN_PROFILE,new VisitorRule("CreateLockdownProfile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_LOCKDOWN_PROFILE,new VisitorRule("DropLockdownProfile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_INMEMORY_JOIN_GROUP,new VisitorRule("CreateInmemoryJoinGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_INMEMORY_JOIN_GROUP,new VisitorRule("AlterInmemoryJoinGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_INMEMORY_JOIN_GROUP,new VisitorRule("DropInmemoryJoinGroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_RESTORE_POINT,new VisitorRule("CreateRestorePoint", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_RESTORE_POINT,new VisitorRule("DropRestorePoint", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_LIBRARY,new VisitorRule("AlterLibrary", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_MATERIALIZED_ZONEMAP,new VisitorRule("AlterMaterializedZonemap", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_JAVA,new VisitorRule("AlterJava", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_AUDIT_POLICY,new VisitorRule("AlterAuditPolicy", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_CLUSTER,new VisitorRule("AlterCluster", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_DISKGROUP,new VisitorRule("AlterDiskgroup", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_HIERARCHY,new VisitorRule("AlterHierarchy", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_INDEX_TYPE,new VisitorRule("AlterIndexType", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.ALTER_LOCKDOWN_PROFILE,new VisitorRule("AlterLockdownProfile", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CURSOR,new VisitorRule("Cursor", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CLOSE,new VisitorRule("Close", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.MOVE,new VisitorRule("Move", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.FETCH,new VisitorRule("Fetch", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CHECKPOINT,new VisitorRule("Checkpoint", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.CLUSTER,new VisitorRule("Cluster", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_ACCESS_METHOD,new VisitorRule("CreateAccessMethod", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DO,new VisitorRule("DoStatement", SqlCommandTypeEnum.DML));
             AddRule(VisitorRuleEnum.PREPARE_TRANSACTION,new VisitorRule("PrepareTransaction", SqlCommandTypeEnum.TCL));
             AddRule(VisitorRuleEnum.REASSIGN_OWNED,new VisitorRule("ReassignOwned", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.CREATE_COLLATION,new VisitorRule("CreateCollation", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_EVENT_TRIGGER,new VisitorRule("CreateEventTrigger", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_FOREIGN_DATA_WRAPPER,new VisitorRule("CreateForeignDataWrapper", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_FOREIGN_TABLE,new VisitorRule("CreateForeignTable", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_GROUP,new VisitorRule("CreateGroup", SqlCommandTypeEnum.DCL));
             AddRule(VisitorRuleEnum.CREATE_MATERIALIZED_VIEW,new VisitorRule("CreateMaterializedView", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_OPERATOR,new VisitorRule("CreateOperator", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.CREATE_POLICY,new VisitorRule("CreatePolicy", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_INDEX_TYPE,new VisitorRule("DropIndexType", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_PLUGGABLE_DATABASE,new VisitorRule("DropPluggableDatabase", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_JAVA,new VisitorRule("DropJava", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_LIBRARY,new VisitorRule("DropLibrary", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_MATERIALIZED_VIEW_LOG,new VisitorRule("DropMaterializedViewLog", SqlCommandTypeEnum.DDL));
             AddRule(VisitorRuleEnum.DROP_MATERIALIZED_ZONEMAP,new VisitorRule("DropMaterializedZonemap", SqlCommandTypeEnum.DDL));
        }

        private static void AddRule(VisitorRuleEnum visitorRuleEnum,VisitorRule visitorRule)
        {
            _rules.Add(visitorRuleEnum, visitorRule);
            _ruleNameIndexs.Add(visitorRule.ContextName,visitorRuleEnum);
        }
        private VisitorRule(string name, SqlCommandTypeEnum sqlCommandType)
        {
            SqlCommandType = sqlCommandType;
            _name = name;
        }

        public static VisitorRuleEnum ValueOf<T>() where T:IParseTree
        {
            return ValueOf(typeof(T));
        }
        public static VisitorRuleEnum ValueOf(Type parseTreeType)
        {
            String parseTreeName = parseTreeType.Name;
            if (_ruleNameIndexs.TryGetValue(parseTreeName, out var visitorRule))
            {
                return visitorRule;
            }
            // foreach (var rule in _rules)
            // {
            //     if (rule.Value.ContextName.Equals(parseTreeName))
            //         return rule.Key;
            // }
            throw new SqlASTVisitorException(parseTreeType);
        }
        public static VisitorRule Get(VisitorRuleEnum r)
        {
            return _rules[r];
        }
        //public static QuoteCharacterEnum GetQuoteCharacter(string value)
        //{
        //    if (string.IsNullOrWhiteSpace(value))
        //        return QuoteCharacterEnum.NONE;
        //    foreach (var quote in _quotes)
        //    {
        //        if (QuoteCharacterEnum.NONE != quote.Key && quote.Value._startDelimiter[0] == value[0])
        //            return quote.Key;
        //    }
        //    return QuoteCharacterEnum.NONE;
        //}
    }

    public enum VisitorRuleEnum
    {
          SELECT,
     TABLE,
     INSERT,
     UPDATE,
     DELETE,
     MERGE,
     REPLACE,
     COPY,
     LOCKTABLE,
     CREATE_TABLE,
     CREATE_AGGREGATE,
     RENAME_TABLE,
     ALTER_TABLE,
     ALTER_TYPE,
     ALTER_AGGREGATE,
     ALTER_COLLATION,
     ALTER_DEFAULT_PRIVILEGES,
     ALTER_FOREIGN_DATA_WRAPPER,
     ALTER_FOREIGN_TABLE,
     DROP_FOREIGN_TABLE,
     ALTER_GROUP,
     ALTER_MATERIALIZED_VIEW,
     ALTER_MATERIALIZED_VIEW_LOG,
     ALTER_PLUGGABLE_DATABASE,
     ALTER_OPERATOR,
     DROP_TABLE,
     TRUNCATE_TABLE,
     CREATE_INDEX,
     ALTER_INDEX,
     DROP_INDEX,
     CREATE_PROCEDURE,
     CREATE_PUBLICATION,
     ALTER_PUBLICATION,
     ALTER_SUBSCRIPTION,
     ALTER_PROCEDURE,
     ALTER_STATEMENT,
     DROP_PROCEDURE,
     DROP_ROUTINE,
     DROP_RULE,
     DROP_STATISTICS,
     DROP_PUBLICATION,
     DROP_SUBSCRIPTION,
     CREATE_FUNCTION,
     ALTER_FUNCTION,
     DROP_CAST,
     DROP_FUNCTION,
     DROP_GROUP,
     CREATE_DATABASE,
     CREATE_DATABASE_LINK,
     ALTER_DATABASE,
     ALTER_DATABASE_LINK,
     DROP_DATABASE,
     DROP_DATABASE_LINK,
     ALTER_DATABASE_DICTIONARY,
     CREATE_DIMENSION,
     ALTER_DIMENSION,
     DROP_DIMENSION,
     ALTER_DIRECTORY,
     DROP_DIRECTORY,
     CREATE_EVENT,
     CREATE_EDITION,
     ALTER_EVENT,
     DROP_EVENT,
     ALTER_INSTANCE,
     CREATE_LOGFILE_GROUP,
     ALTER_LOGFILE_GROUP,
     DROP_LOGFILE_GROUP,
     CREATE_SERVER,
     CREATE_SYNONYM,
     DROP_SYNONYM,
     CREATE_DIRECTORY,
     ALTER_SERVER,
     ALTER_STATISTICS,
     ALTER_SESSION,
     ALTER_SYSTEM,
     DROP_SERVER,
     CREATE_TRIGGER,
     ALTER_TRIGGER,
     DROP_TRIGGER,
     DROP_EVENT_TRIGGER,
     CREATE_VIEW,
     ALTER_VIEW,
     DROP_PACKAGE,
     ALTER_PACKAGE,
     DROP_VIEW,
     ANALYZE,
     CREATE_SEQUENCE,
     ALTER_SEQUENCE,
     DROP_SEQUENCE,
     ALTER_SYNONYM,
     PREPARE,
     EXECUTE_STMT,
     DEALLOCATE,
     CREATE_TABLESPACE,
     ALTER_TABLESPACE,
     DROP_TABLESPACE,
     DROP_TEXT_SEARCH,
     ASSOCIATE_STATISTICS,
     DISASSOCIATE_STATISTICS,
     AUDIT,
     NOAUDIT,
     COMMENT,
     FLASHBACK_DATABASE,
     FLASHBACK_TABLE,
     PURGE,
     RENAME,
     ALTER_ROUTINE,
     CREATE_EXTENSION,
     ALTER_EXTENSION,
     DROP_EXTENSION,
     ALTER_RULE,
     DECLARE,
     DISCARD,
     LISTEN,
     NOTIFY,
     REFRESH_MATERIALIZED_VIEW,
     REINDEX,
     SECURITY_LABEL,
     UNLISTEN,
     SET_CONSTRAINTS,
     SET_TRANSACTION,
     SET_IMPLICIT_TRANSACTIONS,
     BEGIN_TRANSACTION,
     BEGIN_DISTRIBUTED_TRANSACTION,
     START_TRANSACTION,
     END,
     SET_AUTOCOMMIT,
     COMMIT,
     COMMIT_WORK,
     ROLLBACK,
     ROLLBACK_WORK,
     SAVEPOINT,
     RELEASE_SAVEPOINT,
     ROLLBACK_TO_SAVEPOINT,
     LOCK,
     UNLOCK,
     COMMIT_PREPARED,
     ROLLBACK_PREPARED,
     GRANT,
     GRANT_ROLE_OR_PRIVILEGE_TO,
     GRANT_ROLE_OR_PRIVILEGE_ON_TO,
     GRANT_PROXY,
     REVOKE,
     REVOKE_FROM,
     REVOKE_ON_FROM,
     CREATE_USER,
     ALTER_USER,
     DROP_USER,
     DENY_USER,
     RENAME_USER,
     SET_USER,
     CREATE_ROLE,
     ALTER_ROLE,
     DROP_ROLE,
     CREATE_LOGIN,
     ALTER_LOGIN,
     DROP_LOGIN,
     SET_DEFAULT_ROLE,
     SET_ROLE,
     SET_PASSWORD,
     REVERT,
     USE,
     DESC,
     HELP,
     EXPLAIN,
     SHOW_DATABASES,
     SHOW_TABLES,
     SHOW_EVENTS,
     SHOW_CHARACTER_SET,
     SHOW_COLLATION,
     SHOW_VARIABLES,
     SHOW_TABLE_STATUS,
     SHOW_COLUMNS,
     SHOW_INDEX,
     SHOW_CREATE_TABLE,
     SHOW_OTHER,
     SHOW_REPLICAS,
     SHOW_REPLICA_STATUS,
     SHOW_SLAVE_HOSTS,
     SHOW_SLAVE_STATUS,
     SHOW_STATUS,
     SHOW,
     SHOW_RELAYLOG_EVENTS,
     SHOW_PROCEDURE_CODE,
     SHOW_OPEN_TABLES,
     SHOW_TRIGGERS,
     SET_VARIABLE,
     SET,
     SET_NAME,
     SET_CHARACTER,
     RESET_PARAMETER,
     VACUUM,
     CREATE_LOADABLE_FUNCTION,
     ANALYZE_TABLE,
     LOAD,
     INSTALL,
     UNINSTALL,
     FLUSH,
     RESTART,
     SHUTDOWN,
     CREATE_RESOURCE_GROUP,
     ALTER_RESOURCE_COST,
     SET_RESOURCE_GROUP,
     BINLOG,
     OPTIMIZE_TABLE,
     CLONE,
     REPAIR_TABLE,
     KILL,
     RESET,
     RESET_PERSIST,
     CACHE_INDEX,
     LOAD_INDEX_INFO,
     CHECK_TABLE,
     CHECKSUM_TABLE,
     DROP_RESOURCE_GROUP,
     ALTER_RESOURCE_GROUP,
     DELIMITER,
     CALL,
     CHANGE_MASTER,
     START_SLAVE,
     STOP_SLAVE,
     XA,
     ABORT,
     CREATE_SCHEMA,
     ALTER_SCHEMA,
     DROP_SCHEMA,
     CREATE_SERVICE,
     ALTER_SERVICE,
     DROP_SERVICE,
     DROP_DOMAIN,
     CREATE_DOMAIN,
     CREATE_RULE,
     CREATE_LANGUAGE,
     ALTER_LANGUAGE,
     DROP_LANGUAGE,
     CREATE_CONVERSION,
     CREATE_CAST,
     CREATE_TYPE,
     DROP_CONVERSION,
     ALTER_DOMAIN,
     ALTER_POLICY,
     ALTER_CONVERSION,
     CREATE_TEXT_SEARCH,
     ALTER_TEXT_SEARCH_CONFIGURATION,
     ALTER_TEXT_SEARCH_DICTIONARY,
     ALTER_TEXT_SEARCH_TEMPLATE,
     ALTER_TEXT_SEARCH_PARSER,
     DROP_POLICY,
     DROP_OWNED,
     DROP_OPERATOR,
     DROP_MATERIALIZED_VIEW,
     DROP_AGGREGATE,
     DROP_COLLATION,
     DROP_FOREIGN_DATA_WRAPPER,
     DROP_TYPE,
     DROP_OPERATOR_CLASS,
     DROP_OPERATOR_FAMILY,
     DROP_ACCESS_METHOD,
     DROP_OUTLINE,
     ALTER_OUTLINE,
     ALTER_ANALYTIC_VIEW,
     DROP_EDITION,
     ALTER_ATTRIBUTE_DIMENSION,
     CREATE_CONTEXT,
     CREATE_SPFILE,
     CREATE_PFILE,
     CREATE_CONTROL_FILE,
     CREATE_FLASHBACK_ARCHIVE,
     ALTER_FLASHBACK_ARCHIVE,
     DROP_FLASHBACK_ARCHIVE,
     CREATE_DISKGROUP,
     DROP_DISKGROUP,
     CREATE_ROLLBACK_SEGMENT,
     DROP_ROLLBACK_SEGMENT,
     CREATE_LOCKDOWN_PROFILE,
     DROP_LOCKDOWN_PROFILE,
     CREATE_INMEMORY_JOIN_GROUP,
     ALTER_INMEMORY_JOIN_GROUP,
     DROP_INMEMORY_JOIN_GROUP,
     CREATE_RESTORE_POINT,
     DROP_RESTORE_POINT,
     ALTER_LIBRARY,
     ALTER_MATERIALIZED_ZONEMAP,
     ALTER_JAVA,
     ALTER_AUDIT_POLICY,
     ALTER_CLUSTER,
     ALTER_DISKGROUP,
     ALTER_HIERARCHY,
     ALTER_INDEX_TYPE,
     ALTER_LOCKDOWN_PROFILE,
     CURSOR,
     CLOSE,
     MOVE,
     FETCH,
     CHECKPOINT,
     CLUSTER,
     CREATE_ACCESS_METHOD,
     DO,
     PREPARE_TRANSACTION,
     REASSIGN_OWNED,
     CREATE_COLLATION,
     CREATE_EVENT_TRIGGER,
     CREATE_FOREIGN_DATA_WRAPPER,
     CREATE_FOREIGN_TABLE,
     CREATE_GROUP,
     CREATE_MATERIALIZED_VIEW,
     CREATE_OPERATOR,
     CREATE_POLICY,
     DROP_INDEX_TYPE,
     DROP_PLUGGABLE_DATABASE,
     DROP_JAVA,
     DROP_LIBRARY,
     DROP_MATERIALIZED_VIEW_LOG,
     DROP_MATERIALIZED_ZONEMAP,
    }
}
