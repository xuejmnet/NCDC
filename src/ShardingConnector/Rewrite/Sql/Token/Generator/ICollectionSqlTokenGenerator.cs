using System;
using System.Collections.Generic;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Rewrite.Sql.Token.SimpleObject;

namespace ShardingConnector.Rewrite.Sql.Token.Generator
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:46:30
* @Email: 326308290@qq.com
*/
    public interface ICollectionSqlTokenGenerator<T>:ISqlTokenGenerator where T:ISqlCommandContext<ISqlCommand>
    {
        ICollection<SqlToken> GenerateSQLTokens(T sqlCommandContext);
    }
}