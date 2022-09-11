using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Sql.Token.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Monday, 12 April 2021 21:38:57
    * @Email: 326308290@qq.com
    */
    public interface IOptionalSqlTokenGenerator:ISqlTokenGenerator
    {
        SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext);
    }
    public interface IOptionalSqlTokenGenerator<in T> : IOptionalSqlTokenGenerator where T : ISqlCommandContext<ISqlCommand>
    {
        SqlToken GenerateSqlToken(T sqlCommandContext);

        SqlToken IOptionalSqlTokenGenerator.GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((T)sqlCommandContext);
        }
    }
}