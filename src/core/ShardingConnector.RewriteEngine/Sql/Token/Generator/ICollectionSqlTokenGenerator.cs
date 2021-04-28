using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Monday, 12 April 2021 21:46:30
    * @Email: 326308290@qq.com
    */
    public interface ICollectionSqlTokenGenerator
    {
        ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext);
    }
    public interface ICollectionSqlTokenGenerator<in T> : ICollectionSqlTokenGenerator where T : ISqlCommandContext<ISqlCommand>
    {
        ICollection<SqlToken> GenerateSqlTokens(T sqlCommandContext);
    }
}