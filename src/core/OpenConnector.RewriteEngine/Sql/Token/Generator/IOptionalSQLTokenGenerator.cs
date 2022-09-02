
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace OpenConnector.RewriteEngine.Sql.Token.Generator
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
    }
}