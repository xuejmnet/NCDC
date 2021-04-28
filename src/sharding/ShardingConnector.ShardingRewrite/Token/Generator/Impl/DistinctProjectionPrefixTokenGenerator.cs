using System;
using System.Linq;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:43:58
* @Email: 326308290@qq.com
*/
    public sealed class DistinctProjectionPrefixTokenGenerator : IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            return new DistinctProjectionPrefixToken(sqlCommandContext.GetProjectionsContext().GetStartIndex());
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((SelectCommandContext)sqlCommandContext);
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext && selectCommandContext.GetProjectionsContext().GetAggregationDistinctProjections().Any();
        }
    }
}