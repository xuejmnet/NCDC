using System;
using System.Collections.Generic;
using System.Text;
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
    * @Date: 2021/4/28 8:23:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RowCountTokenGenerator: IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            var pagination = sqlCommandContext.GetPaginationContext();
            ShardingAssert.CantBeNull(pagination.GetRowCountSegment(), "row count segment is required");
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
