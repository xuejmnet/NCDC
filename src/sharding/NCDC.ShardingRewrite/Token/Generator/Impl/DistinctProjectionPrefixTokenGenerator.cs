using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl
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