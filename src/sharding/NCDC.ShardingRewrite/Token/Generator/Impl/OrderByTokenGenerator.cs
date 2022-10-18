using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Dialect.Handler.DML;
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
    * @Date: Tuesday, 27 April 2021 21:57:49
    * @Email: 326308290@qq.com
    */
    public sealed class OrderByTokenGenerator : IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            var result = new OrderByToken(GetGenerateOrderByStartIndex(sqlCommandContext));

            foreach (var orderByItem in sqlCommandContext.GetOrderByContext().GetItems())
            {
                string columnLabel;
                if (orderByItem.GetSegment() is ColumnOrderByItemSegment columnOrderByItemSegment)
                {
                    var quoteCharacterEnum = columnOrderByItemSegment.GetColumn().IdentifierValue.QuoteCharacter;
                    QuoteCharacter quoteCharacter = QuoteCharacter.Get(quoteCharacterEnum);
                    columnLabel = quoteCharacter.GetStartDelimiter() + columnOrderByItemSegment.GetExpression() + quoteCharacter.GetEndDelimiter();
                }
                else if (orderByItem.GetSegment() is ExpressionOrderByItemSegment expressionOrderByItemSegment)
                {
                    columnLabel = expressionOrderByItemSegment.GetExpression();
                }
                else
                {
                    columnLabel = $"{orderByItem.GetIndex()}"; // String.valueOf(each.getIndex());
                }
                result.ColumnLabels.Add(columnLabel);
                result.OrderDirections.Add(orderByItem.GetSegment().GetOrderDirection());
            }
            return result;
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((SelectCommandContext)sqlCommandContext);
        }

    
        private int GetGenerateOrderByStartIndex( SelectCommandContext selectCommandContext) {
            var sqlCommand = selectCommandContext.GetSqlCommand();
            int stopIndex;
            if (sqlCommand.Having is not null) {
                stopIndex = sqlCommand.Having.StopIndex;
            } else if (sqlCommand.GroupBy is not null) {
                stopIndex = sqlCommand.GroupBy.StopIndex;
            } else if (sqlCommand.Where is not null) {
                stopIndex = sqlCommand.Where.StopIndex;
            } else {
                stopIndex = selectCommandContext.GetAllTables().Max(o=>o.StopIndex);
            }
            return stopIndex + 1;
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext && selectCommandContext.GetOrderByContext().IsGenerated();
        }
    }
}