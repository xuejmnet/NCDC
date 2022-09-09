using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.Sharding.Rewrites.Token.SimpleObject;

namespace OpenConnector.Sharding.Rewrites.Token.Generator.Impl
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