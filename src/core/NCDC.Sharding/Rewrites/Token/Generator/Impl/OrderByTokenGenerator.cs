using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Constant;
using NCDC.CommandParser.Segment.DML.Order.Item;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.Sharding.Rewrites.Sql.Token.Generator;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Token.SimpleObject;

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
            var result = new OrderByToken(GenerateOrderByIndex(sqlCommandContext));

            foreach (var orderByItem in sqlCommandContext.GetOrderByContext().GetItems())
            {
                string columnLabel;
                if (orderByItem.GetSegment() is ColumnOrderByItemSegment columnOrderByItemSegment)
                {
                    var quoteCharacterEnum = columnOrderByItemSegment.GetColumn().GetIdentifier().GetQuoteCharacter();
                    QuoteCharacter quoteCharacter = QuoteCharacter.Get(quoteCharacterEnum);
                    columnLabel = quoteCharacter.GetStartDelimiter() + columnOrderByItemSegment.GetText() + quoteCharacter.GetEndDelimiter();
                }
                else if (orderByItem.GetSegment() is ExpressionOrderByItemSegment expressionOrderByItemSegment)
                {
                    columnLabel = expressionOrderByItemSegment.GetText();
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

        private int GenerateOrderByIndex(SelectCommandContext selectCommandContext)
        {
            if (selectCommandContext.GetGroupByContext().GetLastIndex() > 0)
            {
                return selectCommandContext.GetGroupByContext().GetLastIndex() + 1;
            }
            var selectCommand = selectCommandContext.GetSqlCommand();
            if (selectCommand.Where != null)
            {
                return selectCommand.Where.GetStopIndex() + 1;
            }
            else
            {
                return selectCommand.GetSimpleTableSegments().Select(o => o.GetStopIndex()).Max() + 1;
            }
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext && selectCommandContext.GetOrderByContext().IsGenerated();
        }
    }
}