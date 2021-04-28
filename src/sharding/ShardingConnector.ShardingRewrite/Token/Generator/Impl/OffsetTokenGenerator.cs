using System;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.DML.Pagination;
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
* @Date: Tuesday, 27 April 2021 21:54:18
* @Email: 326308290@qq.com
*/
    public sealed class OffsetTokenGenerator:IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            var pagination = sqlCommandContext.GetPaginationContext();
            ShardingAssert.CantBeNull(pagination.GetOffsetSegment(), "offset segment is required");
            return new OffsetToken(pagination.GetOffsetSegment().GetStartIndex(), pagination.GetOffsetSegment().GetStopIndex(), pagination.GetRevisedOffset());
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand>  sqlCommandContext)
        {
            return GenerateSqlToken((SelectCommandContext)sqlCommandContext);

        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        { return sqlCommandContext is SelectCommandContext selectCommandContext
                && selectCommandContext.GetPaginationContext().GetOffsetSegment()!=null
                && selectCommandContext.GetPaginationContext().GetOffsetSegment() is INumberLiteralPaginationValueSegment;

        }
    }
}