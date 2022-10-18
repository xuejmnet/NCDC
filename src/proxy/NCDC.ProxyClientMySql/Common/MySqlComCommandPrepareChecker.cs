using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DCL;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Command.TCL;

namespace NCDC.ProxyClientMySql.Common;

public static class MySqlComCommandPrepareChecker
{
    private static readonly ISet<Type> SQL_COMMANDS_ALLOWED;
    static MySqlComCommandPrepareChecker()
    {
        SQL_COMMANDS_ALLOWED = new HashSet<Type>();
        SQL_COMMANDS_ALLOWED.Add(typeof(AlterTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CallCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CommitCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateIndexCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropIndexCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateDatabaseCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropDatabaseCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(CreateTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DropTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DeleteCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(DoCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(GrantCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(InsertCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RenameTableCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(RevokeCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(SelectCommand));
        SQL_COMMANDS_ALLOWED.Add(typeof(TruncateCommand));
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