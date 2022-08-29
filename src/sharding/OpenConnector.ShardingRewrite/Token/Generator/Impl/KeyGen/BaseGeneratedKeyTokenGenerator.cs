using System;

using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.RewriteEngine.Sql.Token.Generator;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace OpenConnector.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:39:40
* @Email: 326308290@qq.com
*/
    public abstract class BaseGeneratedKeyTokenGenerator:IOptionalSqlTokenGenerator<InsertCommandContext>
    {
        public abstract SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext);

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((InsertCommandContext) sqlCommandContext);
        }


        public abstract bool IsGenerateSqlToken(InsertCommand insertCommand);

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is InsertCommandContext insertCommandContext && insertCommandContext.GetGeneratedKeyContext()!=null
                                                                                  && insertCommandContext.GetGeneratedKeyContext().IsGenerated() && IsGenerateSqlToken(insertCommandContext.GetSqlCommand());
        }
    }
}