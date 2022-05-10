
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:08:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISqlTokenGenerator
    {
        bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext);
    }
}
