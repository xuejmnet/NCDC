using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.Generator;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.Generator.Impl.KeyGen
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



        public abstract bool IsGenerateSqlToken(InsertCommand insertCommand);

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is InsertCommandContext insertCommandContext && insertCommandContext.GetGeneratedKeyContext()!=null
                                                                                  && insertCommandContext.GetGeneratedKeyContext().IsGenerated() && IsGenerateSqlToken(insertCommandContext.GetSqlCommand());
        }
    }
}