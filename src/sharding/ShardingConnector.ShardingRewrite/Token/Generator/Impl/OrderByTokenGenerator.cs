using System;
using System.Linq;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:57:49
* @Email: 326308290@qq.com
*/
    public sealed class OrderByTokenGenerator:IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        { 
            OrderByToken result = new OrderByToken(generateOrderByIndex(selectStatementContext));
            String columnLabel;
            for (OrderByItem each : selectStatementContext.getOrderByContext().getItems()) {
                if (each.getSegment() instanceof ColumnOrderByItemSegment) {
                    ColumnOrderByItemSegment columnOrderByItemSegment = (ColumnOrderByItemSegment) each.getSegment();
                    QuoteCharacter quoteCharacter = columnOrderByItemSegment.getColumn().getIdentifier().getQuoteCharacter();
                    columnLabel = quoteCharacter.getStartDelimiter() + columnOrderByItemSegment.getText() + quoteCharacter.getEndDelimiter();
                } else if (each.getSegment() instanceof ExpressionOrderByItemSegment) {
                    columnLabel = ((ExpressionOrderByItemSegment) each.getSegment()).getText();
                } else {
                    columnLabel = String.valueOf(each.getIndex());
                }
                result.getColumnLabels().add(columnLabel);
                result.getOrderDirections().add(each.getSegment().getOrderDirection());
            }
            return result;
        }
    
        private int GenerateOrderByIndex(SelectCommandContext selectCommandContext) {
            if (selectCommandContext.GetGroupByContext().GetLastIndex() > 0) {
                return selectCommandContext.GetGroupByContext().GetLastIndex() + 1;
            }
            var selectCommand = selectCommandContext.GetSqlCommand();
            if (selectCommand.Where!=null) {
                return selectCommand.Where.GetStopIndex() + 1;
            } else {
                return selectCommand.GetSimpleTableSegments().Select(o=>o.GetStopIndex()).Max() + 1;
            }
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext && selectCommandContext.GetOrderByContext().IsGenerated();

        }
    }
}