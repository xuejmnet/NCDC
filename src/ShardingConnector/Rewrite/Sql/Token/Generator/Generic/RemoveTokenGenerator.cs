using System;
using System.Collections.Generic;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite.Sql.Token.SimpleObject;

namespace ShardingConnector.Rewrite.Sql.Token.Generator.Generic
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
            throw new NotImplementedException();
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            throw new NotImplementedException();
        }
    }
}