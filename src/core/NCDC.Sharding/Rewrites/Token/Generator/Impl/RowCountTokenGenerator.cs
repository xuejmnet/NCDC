﻿using OpenConnector.Base;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Segment.DML.Pagination;
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
