using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.CommandParser.Segment.Generic;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject.Generic;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:51:39
* @Email: 326308290@qq.com
*/
    public sealed class RemoveTokenGenerator:ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>>
    {
        // public ICollection<SqlToken> GenerateSQLTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        // { 
        //     if (sqlCommandContext.GetSqlCommand() is ShowTablesStatement) {
        //         Preconditions.checkState(((ShowTablesStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent());
        //         RemoveAvailable removeAvailable = ((ShowTablesStatement) sqlStatementContext.getSqlStatement()).getFromSchema().get();
        //         return Collections.singletonList(new RemoveToken(removeAvailable.getStartIndex(), removeAvailable.getStopIndex()));
        //     }
        //     if (sqlStatementContext.getSqlStatement() instanceof ShowTableStatusStatement) {
        //         Preconditions.checkState(((ShowTableStatusStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent());
        //         RemoveAvailable removeAvailable = ((ShowTableStatusStatement) sqlStatementContext.getSqlStatement()).getFromSchema().get();
        //         return Collections.singletonList(new RemoveToken(removeAvailable.getStartIndex(), removeAvailable.getStopIndex()));
        //     }
        //     if (sqlStatementContext.getSqlStatement() instanceof ShowColumnsStatement) {
        //         Preconditions.checkState(((ShowColumnsStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent());
        //         RemoveAvailable removeAvailable = ((ShowColumnsStatement) sqlStatementContext.getSqlStatement()).getFromSchema().get();
        //         return Collections.singletonList(new RemoveToken(removeAvailable.getStartIndex(), removeAvailable.getStopIndex()));
        //     }
        //     return Collections.emptyList();
        // }
        //
        // public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        // {if (sqlStatementContext.getSqlStatement() instanceof ShowTablesStatement) {
        //         return ((ShowTablesStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent();
        //     }
        //     if (sqlStatementContext.getSqlStatement() instanceof ShowTableStatusStatement) {
        //         return ((ShowTableStatusStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent();
        //     }
        //     if (sqlStatementContext.getSqlStatement() instanceof ShowColumnsStatement) {
        //         return ((ShowColumnsStatement) sqlStatementContext.getSqlStatement()).getFromSchema().isPresent();
        //     }
        //     return false;
        // }
        public ICollection<SqlToken> GenerateSQLTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (sqlCommandContext.GetSqlCommand() is ShowTablesCommand showTablesCommand)
            {
                if (showTablesCommand.GetFromSchema() == null)
                    throw new ArgumentNullException("showTablesCommand.GetFromSchema");

                var removeAvailable = showTablesCommand.GetFromSchema();
                return new List<SqlToken>(){ new RemoveToken(removeAvailable.GetStartIndex(), removeAvailable.GetStopIndex()) };
            }
            if (sqlCommandContext.GetSqlCommand() is ShowTableStatusCommand showTableStatusCommand) {
                if(showTableStatusCommand.GetFromSchema()==null)
                    throw new ArgumentNullException("showTableStatusCommand.GetFromSchema");
                var removeAvailable = showTableStatusCommand.GetFromSchema();
                return new List<SqlToken>(){ new RemoveToken(removeAvailable.GetStartIndex(), removeAvailable.GetStopIndex()) };
            }
            if (sqlCommandContext.GetSqlCommand() is ShowColumnsCommand showColumnsCommand)
            {
                if (showColumnsCommand.GetFromSchema() == null)
                    throw new ArgumentNullException("showColumnsCommand.GetFromSchema");
                var removeAvailable = showColumnsCommand.GetFromSchema();
                return new List<SqlToken>(){ new RemoveToken(removeAvailable.GetStartIndex(), removeAvailable.GetStopIndex()) };
            }
            return new List<SqlToken>(0);
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (sqlCommandContext.GetSqlCommand() is ShowTablesCommand showTablesCommand) {
                return showTablesCommand.GetFromSchema()!=null;
            }
            if (sqlCommandContext.GetSqlCommand() is ShowTableStatusCommand showTableStatusCommand) {
                return showTableStatusCommand.GetFromSchema() != null;
            }
            if (sqlCommandContext.GetSqlCommand() is ShowColumnsCommand showColumnsCommand) {
                return showColumnsCommand.GetFromSchema() != null;
            }
            return false;
        }
    }
}