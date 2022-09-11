using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using NCDC.CommandParser.Command;
using OpenConnector.Exceptions;

namespace OpenConnector.ParserEngine.Core.Visitor
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
        public SqlCommandTypeEnum SqlCommandType { get; }
        private readonly string _name;

        private string ContextName => _name + "Context";

        static VisitorRule()
        {
            _rules.Add(VisitorRuleEnum.SELECT,new VisitorRule("Select", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.INSERT,new VisitorRule("Insert", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.UPDATE,new VisitorRule("Update", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.DELETE,new VisitorRule("Delete", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.REPLACE,new VisitorRule("Replace", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.CREATE_TABLE,new VisitorRule("CreateTable", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.ALTER_TABLE,new VisitorRule("AlterTable", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.DROP_TABLE,new VisitorRule("DropTable", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.TRUNCATE_TABLE,new VisitorRule("TruncateTable", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.CREATE_INDEX,new VisitorRule("CreateIndex", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.ALTER_INDEX,new VisitorRule("AlterIndex", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.DROP_INDEX,new VisitorRule("DropIndex", SqlCommandTypeEnum.DDL));
            _rules.Add(VisitorRuleEnum.SET_TRANSACTION,new VisitorRule("SetTransaction", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.SET_IMPLICIT_TRANSACTIONS,new VisitorRule("SetImplicitTransactions", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.BEGIN_TRANSACTION,new VisitorRule("BeginTransaction", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.SET_AUTOCOMMIT,new VisitorRule("SetAutoCommit", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.COMMIT,new VisitorRule("Commit", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.ROLLBACK,new VisitorRule("Rollback", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.SAVE_POINT,new VisitorRule("Savepoint", SqlCommandTypeEnum.TCL));
            _rules.Add(VisitorRuleEnum.GRANT,new VisitorRule("Grant", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.REVOKE,new VisitorRule("Revoke", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.CREATE_USER,new VisitorRule("CreateUser", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.ALTER_USER,new VisitorRule("AlterUser", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.DROP_USER,new VisitorRule("DropUser", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.DENY_USER,new VisitorRule("Deny", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.RENAME_USER,new VisitorRule("RenameUser", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.CREATE_ROLE,new VisitorRule("CreateRole", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.ALTER_ROLE,new VisitorRule("AlterRole", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.DROP_ROLE,new VisitorRule("DropRole", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.CREATE_LOGIN,new VisitorRule("CreateLogin", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.ALTER_LOGIN,new VisitorRule("AlterLogin", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.DROP_LOGIN,new VisitorRule("DropLogin", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.SET_DEFAULT_ROLE,new VisitorRule("SetDefaultRole", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.SET_ROLE,new VisitorRule("SetRole", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.SET_PASSWORD,new VisitorRule("SetPassword", SqlCommandTypeEnum.DCL));
            _rules.Add(VisitorRuleEnum.USE,new VisitorRule("Use", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.DESC,new VisitorRule("Desc", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_DATABASES,new VisitorRule("ShowDatabases", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_TABLES,new VisitorRule("ShowTables", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_TABLE_STATUS,new VisitorRule("ShowTableStatus", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_COLUMNS,new VisitorRule("ShowColumns", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_INDEX,new VisitorRule("ShowIndex", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_CREATE_TABLE,new VisitorRule("ShowCreateTable", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW_OTHER,new VisitorRule("ShowOther", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SHOW,new VisitorRule("Show", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SET_VARIABLE,new VisitorRule("SetVariable", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.SET,new VisitorRule("Set", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.RESET_PARAMETER,new VisitorRule("ResetParameter", SqlCommandTypeEnum.DAL));
            _rules.Add(VisitorRuleEnum.CALL,new VisitorRule("Call", SqlCommandTypeEnum.DML));
            _rules.Add(VisitorRuleEnum.CHANGE_MASTER,new VisitorRule("ChangeMaster", SqlCommandTypeEnum.RL));
            _rules.Add(VisitorRuleEnum.START_SLAVE,new VisitorRule("StartSlave", SqlCommandTypeEnum.RL));
            _rules.Add(VisitorRuleEnum.STOP_SLAVE,new VisitorRule("StopSlave", SqlCommandTypeEnum.RL));
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
            foreach (var rule in _rules)
            {
                if (rule.Value.ContextName.Equals(parseTreeName))
                    return rule.Key;
            }
            throw new ShardingException($"Can not find visitor rule: `{parseTreeName}`");
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
        INSERT,
        UPDATE,
        DELETE,
        REPLACE,
        CREATE_TABLE,
        ALTER_TABLE,
        DROP_TABLE,
        TRUNCATE_TABLE,
        CREATE_INDEX,
        ALTER_INDEX,
        DROP_INDEX,
        SET_TRANSACTION,
        SET_IMPLICIT_TRANSACTIONS,
        BEGIN_TRANSACTION,
        SET_AUTOCOMMIT,
        COMMIT,
        ROLLBACK,
        SAVE_POINT,
        GRANT,
        REVOKE,
        CREATE_USER,
        ALTER_USER,
        DROP_USER,
        DENY_USER,
        RENAME_USER,
        CREATE_ROLE,
        ALTER_ROLE,
        DROP_ROLE,
        CREATE_LOGIN,
        ALTER_LOGIN,
        DROP_LOGIN,
        SET_DEFAULT_ROLE,
        SET_ROLE,
        SET_PASSWORD,
        USE,
        DESC,
        SHOW_DATABASES,
        SHOW_TABLES,
        SHOW_TABLE_STATUS,
        SHOW_COLUMNS,
        SHOW_INDEX,
        SHOW_CREATE_TABLE,
        SHOW_OTHER,
        SHOW,
        SET_VARIABLE,
        SET,
        RESET_PARAMETER,
        CALL,
        CHANGE_MASTER,
        START_SLAVE,
        STOP_SLAVE
    }
}
