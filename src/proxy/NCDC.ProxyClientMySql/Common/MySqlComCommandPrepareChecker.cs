using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DAL.Dialect;
using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using NCDC.CommandParser.Command.DCL;
using NCDC.CommandParser.Command.DDL;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Command.RL;
using NCDC.CommandParser.Command.TCL;

namespace NCDC.ProxyClientMySql.Common;

public static class MySqlComCommandPrepareChecker
{
    private static readonly ISet<Type> SQL_COMMANDS_ALLOWED;
    static MySqlComCommandPrepareChecker()
    {
        SQL_COMMANDS_ALLOWED = new HashSet<Type>();
        SQL_COMMANDS_ALLOWED.Add(typeof(AlterTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(AlterUserCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(AnalyzeTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CacheIndexCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CallCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ChangeMasterCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ChecksumTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CommitCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateIndexCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropIndexCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateDatabaseCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropDatabaseCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateUserCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RenameUserCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropUserCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateViewCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropViewCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DeleteCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DoCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(FlushCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(GrantCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(InsertCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(InstallPluginCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(KillCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(LoadIndexInfoCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(OptimizeTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RenameTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RepairTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ReplaceCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ResetCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RevokeCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(SelectCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(SetCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowWarningsCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowErrorsCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowBinlogCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowCreateProcedureCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowCreateFunctionCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowCreateEventCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowCreateTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowCreateViewCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowBinaryLogsCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(ShowStatusCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(StartSlaveCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(StopSlaveStatement));
        SQL_COMMANDS_ALLOWED.Add(typeof(TruncateCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(UninstallPluginCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(UpdateCommand));
    }

    public static bool IsCommandAllowed(Type commandType)
    {
        return SQL_COMMANDS_ALLOWED.Contains(commandType);
    }

    public static bool IsCommandAllowed(ISqlCommand sqlCommand)
    {
        return SQL_COMMANDS_ALLOWED.Contains(sqlCommand.GetType());
    }
}