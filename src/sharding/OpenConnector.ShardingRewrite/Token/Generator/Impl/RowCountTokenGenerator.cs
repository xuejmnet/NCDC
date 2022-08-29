using System;
using System.Collections.Generic;
using System.Text;

using OpenConnector.Base;
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
    * @Date: 2021/4/28 8:23:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RowCountTokenGenerator: IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            var pagination = sqlCommandContext.GetPaginationContext();
            ShardingAssert.ShouldBeNotNull(pagination.GetRowCountSegment(), "row count segment is required");
            return new RowCountToken(pagination.GetRowCountSegment().GetStartIndex(), pagination.GetRowCountSegment().GetStopIndex(), pagination.GetRevisedRowCount(sqlCommandContext));
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((SelectCommandContext)sqlCommandContext);
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext
                && selectCommandContext.GetPaginationContext().GetRowCountSegment()!=null
                && selectCommandContext.GetPaginationContext().GetRowCountSegment() is INumberLiteralPaginationValueSegment;

        }
    }
}
