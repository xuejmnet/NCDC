using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.Generator;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.Generator.Impl
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