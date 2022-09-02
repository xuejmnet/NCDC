using System;
using OpenConnector.Base;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Segment.DML.Pagination;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.RewriteEngine.Sql.Token.Generator;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.ShardingRewrite.Token.SimpleObject;

namespace OpenConnector.ShardingRewrite.Token.Generator.Impl
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