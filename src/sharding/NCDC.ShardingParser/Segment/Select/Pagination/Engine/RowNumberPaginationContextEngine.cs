using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.ShardingParser.Segment.Select.Projection;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Select.Pagination.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:54:40
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RowNumberPaginationContextEngine
    {
        // TODO recognize database type, only oracle and sqlserver can use row number
        private static readonly ICollection<String> ROW_NUMBER_IDENTIFIERS = new HashSet<String>();

        static RowNumberPaginationContextEngine()
        {
            ROW_NUMBER_IDENTIFIERS.Add("rownum");
            ROW_NUMBER_IDENTIFIERS.Add("ROW_NUMBER");
        }

        /**
         * Create pagination context.
         * 
         * @param andPredicates and predicates
         * @param projectionsContext projections context
         * @param parameters SQL parameters
         * @return pagination context
         */
        public PaginationContext CreatePaginationContext(ICollection<AndPredicateSegment> andPredicates, ProjectionsContext projectionsContext, ParameterContext parameterContext)
        {
            var rowNumberAlias = IsRowNumberAlias(projectionsContext);
            if (rowNumberAlias == null)
            {
                return new PaginationContext(null, null, parameterContext);
            }
            ICollection<PredicateSegment> rowNumberPredicates = GetRowNumberPredicates(andPredicates, rowNumberAlias);
            return !rowNumberPredicates.Any() ? new PaginationContext(null, null, parameterContext) : CreatePaginationWithRowNumber(rowNumberPredicates, parameterContext);
        }

        private ICollection<PredicateSegment> GetRowNumberPredicates(ICollection<AndPredicateSegment> andPredicates, string rowNumberAlias)
        {
            ICollection<PredicateSegment> result = new LinkedList<PredicateSegment>();
            foreach (var andPredicate in andPredicates)
            {
                foreach (var predicate in andPredicate.GetPredicates())
                {
                    if (IsRowNumberColumn(predicate, rowNumberAlias) && IsCompareCondition(predicate))
                    {
                        result.Add(predicate);
                    }
                }
            }
            return result;
        }

        private string IsRowNumberAlias(ProjectionsContext projectionsContext)
        {
            foreach (var item in ROW_NUMBER_IDENTIFIERS)
            {
                var alias = projectionsContext.FindAlias(item);
                if (alias != null)
                {
                    return alias;
                }

            }
            return null;
        }

        private bool IsRowNumberColumn(PredicateSegment predicate, string rowNumberAlias)
        {
            return ROW_NUMBER_IDENTIFIERS.Contains(predicate.GetColumn().GetIdentifier().GetValue()) || predicate.GetColumn().GetIdentifier().GetValue().EqualsIgnoreCase(rowNumberAlias);
        }

        private bool IsCompareCondition(PredicateSegment predicate)
        {
            if (predicate.GetPredicateRightValue() is PredicateCompareRightValue predicateCompareRightValue)
            {
                var @operator = predicateCompareRightValue.GetOperator();
                return "<".Equals(@operator) || "<=".Equals(@operator) || ">".Equals(@operator) || ">=".Equals(@operator);
            }
            return false;
        }

        private PaginationContext CreatePaginationWithRowNumber(ICollection<PredicateSegment> rowNumberPredicates, ParameterContext parameterContext)
        {
            RowNumberValueSegment offset = null;
            RowNumberValueSegment rowCount = null;
            foreach (var rowNumberPredicate in rowNumberPredicates)
            {
                var expression = ((PredicateCompareRightValue)rowNumberPredicate.GetPredicateRightValue()).GetExpression();
                switch (((PredicateCompareRightValue)rowNumberPredicate.GetPredicateRightValue()).GetOperator())
                {
                    case ">":
                        offset = CreateRowNumberValueSegment(expression, false);
                        break;
                    case ">=":
                        offset = CreateRowNumberValueSegment(expression, true);
                        break;
                    case "<":
                        rowCount = CreateRowNumberValueSegment(expression, false);
                        break;
                    case "<=":
                        rowCount = CreateRowNumberValueSegment(expression, true);
                        break;
                    default:
                        break;
                }
            }
            return new PaginationContext(offset, rowCount, parameterContext);
        }

        private RowNumberValueSegment CreateRowNumberValueSegment(IExpressionSegment expression, bool boundOpened)
        {
            int startIndex = expression.GetStartIndex();
            int stopIndex = expression.GetStopIndex();
            if (expression is LiteralExpressionSegment literalExpressionSegment)
            {
                return new NumberLiteralRowNumberValueSegment(startIndex, stopIndex,
                    (int)((LiteralExpressionSegment)expression).GetLiterals(), boundOpened);
            }

            var parameterMarkerExpression = ((ParameterMarkerExpressionSegment)expression);
            return new ParameterMarkerRowNumberValueSegment(startIndex, stopIndex, parameterMarkerExpression.GetParameterMarkerIndex(), parameterMarkerExpression.GetParameterName(), boundOpened);
        }
    }
}
