using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen
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



        public abstract bool IsGenerateSqlToken(InsertCommandContext insertCommandContext);

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is InsertCommandContext insertCommandContext && insertCommandContext.GetGeneratedKeyContext()!=null
                                                                                  && insertCommandContext.GetGeneratedKeyContext().IsGenerated() && IsGenerateSqlToken(insertCommandContext);
        }
    }
}