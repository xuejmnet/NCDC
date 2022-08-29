using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.CommandParser.Segment.DML.Pagination;
using OpenConnector.CommandParser.Segment.DML.Pagination.RowNumber;
using OpenConnector.CommandParser.Segment.DML.Pagination.Top;
using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.CommandParser.Segment.Predicate;
using OpenConnector.Extensions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.CommandParserBinder.Segment.Select.Pagination.Engine
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
        public PaginationContext CreatePaginationContext(TopProjectionSegment topProjectionSegment, ICollection<AndPredicateSegment> andPredicates, ParameterContext parameterContext)
        {
            var rowNumberPredicate = GetRowNumberPredicate(andPredicates, topProjectionSegment.GetAlias());
            var offset = rowNumberPredicate!=null ? CreateOffsetWithRowNumber(rowNumberPredicate) : null;
            var rowCount = topProjectionSegment.GetTop();
            return new PaginationContext(offset, rowCount, parameterContext);
        }

        private PredicateSegment GetRowNumberPredicate(ICollection<AndPredicateSegment> andPredicates, string rowNumberAlias)
        {
            foreach (var andPredicate in andPredicates)
            {
                foreach (var predicate in andPredicate.GetPredicates())
                {
                    if (IsRowNumberColumn(predicate, rowNumberAlias) && IsCompareCondition(predicate))
                        return predicate;
                }   
            }
            return null;
        }

        private bool IsRowNumberColumn(PredicateSegment predicate, string rowNumberAlias)
        {
            return predicate.GetColumn().GetIdentifier().GetValue().EqualsIgnoreCase(rowNumberAlias);
        }

        private bool IsCompareCondition(PredicateSegment predicate)
        {
            var predicateRightValue=predicate.GetPredicateRightValue();
            if (predicateRightValue is PredicateCompareRightValue predicateCompareRightValue) {
                var @operator= predicateCompareRightValue.GetOperator();
                return ">".Equals(@operator) || ">=".Equals(@operator);
            }
            return false;
        }

        private IPaginationValueSegment CreateOffsetWithRowNumber(PredicateSegment predicateSegment)
        {
            IExpressionSegment expression = ((PredicateCompareRightValue)predicateSegment.GetPredicateRightValue()).GetExpression();
            switch (((PredicateCompareRightValue)predicateSegment.GetPredicateRightValue()).GetOperator())
            {
                case ">":
                    return CreateRowNumberValueSegment(expression, false);
                case ">=":
                    return CreateRowNumberValueSegment(expression, true);
                default:
                    return null;
            }
        }

        private RowNumberValueSegment CreateRowNumberValueSegment(IExpressionSegment expression, bool boundOpened)
        {
            int startIndex = expression.GetStartIndex();
            int stopIndex = expression.GetStopIndex();
            if (expression is LiteralExpressionSegment literalExpressionSegment)
            {
                return new NumberLiteralRowNumberValueSegment(startIndex, stopIndex,
                    (int)literalExpressionSegment.GetLiterals(), boundOpened);
            }

            var parameterMarkerExpression = ((ParameterMarkerExpressionSegment)expression);
            return new ParameterMarkerRowNumberValueSegment(startIndex, stopIndex,
                parameterMarkerExpression.GetParameterMarkerIndex(), parameterMarkerExpression.GetParameterName(), boundOpened);
        }
    }
}
