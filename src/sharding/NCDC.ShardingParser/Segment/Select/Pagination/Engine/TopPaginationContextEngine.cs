using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Pagination;
using NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber;
using NCDC.CommandParser.Common.Segment.DML.Pagination.Top;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.CommandParser.Common.Util;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Select.Pagination.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:32:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public sealed class TopPaginationContextEngine
    {
        /**
         * Create pagination context.
         * 
         * @param topProjectionSegment top projection segment
         * @param andPredicates and predicates
         * @param parameters SQL parameters
         * @return pagination context
         */
        public PaginationContext CreatePaginationContext(TopProjectionSegment topProjectionSegment, ICollection<IExpressionSegment> expressions, ParameterContext parameterContext)
        {
            var andPredicates = expressions.SelectMany(o=>ExpressionExtractUtil.GetAndPredicateSegments(o)).ToList();
            var rowNumberPredicate =andPredicates.IsNotEmpty()? GetRowNumberPredicate(andPredicates, topProjectionSegment.GetAlias()):null;
            var offset = rowNumberPredicate!=null ? CreateOffsetWithRowNumber(rowNumberPredicate) : null;
            var rowCount = topProjectionSegment.GetTop();
            return new PaginationContext(offset, rowCount, parameterContext);
        }

        private IExpressionSegment? GetRowNumberPredicate(ICollection<AndPredicateSegment> andPredicates, string rowNumberAlias)
        {
            foreach (var andPredicate in andPredicates)
            {
                foreach (var predicate in andPredicate.Predicates)
                {
                    if (IsRowNumberColumn(predicate, rowNumberAlias) && IsCompareCondition(predicate))
                        return predicate;
                }   
            }
            return null;
        }

        private bool IsRowNumberColumn(IExpressionSegment predicate, string rowNumberAlias)
        {
            if (predicate is BinaryOperationExpression binaryOperationExpression)
            {
                return  binaryOperationExpression.Left is ColumnSegment columnSegment&& columnSegment.IdentifierValue.Value.EqualsIgnoreCase(rowNumberAlias);
            }

            return false;
        }

        private bool IsCompareCondition(IExpressionSegment predicate)
        {
            if (predicate is BinaryOperationExpression binaryOperationExpression)
            {
                var @operator = binaryOperationExpression.Operator;
                return ">".Equals(@operator) || ">=".Equals(@operator);
            }
            return false;
        }

        private IPaginationValueSegment? CreateOffsetWithRowNumber(IExpressionSegment predicateSegment)
        {
            if (!(predicateSegment is BinaryOperationExpression binaryOperationExpression))
            {
                return null;
            }

            var @operator = binaryOperationExpression.Operator;
            switch (@operator)
            {
                case ">":
                    return CreateRowNumberValueSegment(binaryOperationExpression, false);
                case ">=":
                    return CreateRowNumberValueSegment(binaryOperationExpression, true);
                default:
                    return null;
            }
        }

        private RowNumberValueSegment CreateRowNumberValueSegment(IExpressionSegment expression, bool boundOpened)
        {
            int startIndex = expression.StartIndex;
            int stopIndex = expression.StopIndex;
            if (expression is LiteralExpressionSegment literalExpressionSegment)
            {
                return new NumberLiteralRowNumberValueSegment(startIndex, stopIndex,
                    (int)literalExpressionSegment.Literals, boundOpened);
            }

            var parameterMarkerExpression = ((ParameterMarkerExpressionSegment)expression);
            return new ParameterMarkerRowNumberValueSegment(startIndex, stopIndex,
                parameterMarkerExpression.ParameterMarkerIndex, parameterMarkerExpression.ParamName, boundOpened);
        }
    }
}
