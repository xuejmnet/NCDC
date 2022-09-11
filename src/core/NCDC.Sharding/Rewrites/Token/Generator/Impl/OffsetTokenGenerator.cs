using OpenConnector.Base;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Segment.DML.Pagination;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.Generator;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.Generator.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:54:18
* @Email: 326308290@qq.com
*/
    public sealed class OffsetTokenGenerator:IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            var pagination = sqlCommandContext.GetPaginationContext();
            ShardingAssert.ShouldBeNotNull(pagination.GetOffsetSegment(), "offset segment is required");
            return new OffsetToken(pagination.GetOffsetSegment().GetStartIndex(), pagination.GetOffsetSegment().GetStopIndex(), pagination.GetRevisedOffset());
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        { return sqlCommandContext is SelectCommandContext selectCommandContext
                && selectCommandContext.GetPaginationContext().GetOffsetSegment()!=null
                && selectCommandContext.GetPaginationContext().GetOffsetSegment() is INumberLiteralPaginationValueSegment;

        }
    }
}