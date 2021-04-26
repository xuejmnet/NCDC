using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:38:57
* @Email: 326308290@qq.com
*/
    public interface IOptionalSQLTokenGenerator<in T>:ISqlTokenGenerator where T:ISqlCommandContext<ISqlCommand>
    {
        SqlToken GenerateSQLToken(T sqlCommandContext);
    }
}