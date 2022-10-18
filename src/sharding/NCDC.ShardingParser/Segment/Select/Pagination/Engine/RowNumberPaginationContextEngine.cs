using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.CommandParser.Common.Util;
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
        public PaginationContext CreatePaginationContext(ICollection<IExpressionSegment> expressions, ProjectionsContext projectionsContext, ParameterContext parameterContext)
        {
            var rowNumberAlias = IsRowNumberAlias(projectionsContext);
            if (rowNumberAlias == null)
            {
                return new PaginationContext(null, null, parameterContext);
            }

            var andPredicates = expressions.SelectMany(o=>ExpressionExtractUtil.GetAndPredicateSegments(o)).ToList();
            var rowNumberPredicates = GetRowNumberPredicates(andPredicates, rowNumberAlias);
            return !rowNumberPredicates.Any() ? new PaginationContext(null, null, parameterContext) : CreatePaginationWithRowNumber(rowNumberPredicates, parameterContext);
        }

        private ICollection<BinaryOperationExpression> GetRowNumberPredicates(ICollection<AndPredicateSegment> andPredicates, string rowNumberAlias)
        {
            ICollection<BinaryOperationExpression> result = new LinkedList<BinaryOperationExpression>();
            foreach (var andPredicate in andPredicates)
            {
                foreach (var predicate in andPredicate.Predicates)
                {
                    if (IsRowNumberColumn(predicate, rowNumberAlias) && IsCompareCondition(predicate))
                    {
                        result.Add((BinaryOperationExpression)predicate);
                    }
                }
            }
            return result;
        }

        private string? IsRowNumberAlias(ProjectionsContext projectionsContext)
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

        private bool IsRowNumberColumn(IExpressionSegment predicate, string rowNumberAlias)
        {
            if (predicate is BinaryOperationExpression binaryOperationExpression)
            {
                if (binaryOperationExpression.Left is ColumnSegment columnSegment)
                {
                    var leftColumnValue = columnSegment.IdentifierValue.Value;
                    return ROW_NUMBER_IDENTIFIERS.Contains(leftColumnValue) ||
                           leftColumnValue.EqualsIgnoreCase(rowNumberAlias);
                }
            }

            return false;
        }

        private bool IsCompareCondition(IExpressionSegment predicate)
        {
            if (predicate is BinaryOperationExpression binaryOperationExpression)
            {
                var @operator = binaryOperationExpression.Operator;
                return "<".Equals(@operator) || "<=".Equals(@operator) || ">".Equals(@operator) || ">=".Equals(@operator);
            }
            return false;
        }

        private PaginationContext CreatePaginationWithRowNumber(ICollection<BinaryOperationExpression> rowNumberPredicates, ParameterContext parameterContext)
        {
            RowNumberValueSegment? offset = null;
            RowNumberValueSegment? rowCount = null;
            foreach (var rowNumberPredicate in rowNumberPredicates)
            {
                var @operator = rowNumberPredicate.Operator;
                switch (@operator)
                {
                    case ">":
                        offset = CreateRowNumberValueSegment(rowNumberPredicate.Right, false);
                        break;
                    case ">=":
                        offset = CreateRowNumberValueSegment(rowNumberPredicate.Right, true);
                        break;
                    case "<":
                        rowCount = CreateRowNumberValueSegment(rowNumberPredicate.Right, false);
                        break;
                    case "<=":
                        rowCount = CreateRowNumberValueSegment(rowNumberPredicate.Right, true);
                        break;
                    default:
                        break;
                }
            }
            return new PaginationContext(offset, rowCount, parameterContext);
        }

        private RowNumberValueSegment CreateRowNumberValueSegment(IExpressionSegment expression, bool boundOpened)
        {
            int startIndex = expression.StartIndex;
            int stopIndex = expression.StopIndex;
            if (expression is LiteralExpressionSegment literalExpressionSegment)
            {
                return new NumberLiteralRowNumberValueSegment(startIndex, stopIndex,
                    (int)((LiteralExpressionSegment)expression).Literals, boundOpened);
            }

            var parameterMarkerExpression = ((ParameterMarkerExpressionSegment)expression);
            return new ParameterMarkerRowNumberValueSegment(startIndex, stopIndex, parameterMarkerExpression.ParameterMarkerIndex, parameterMarkerExpression.ParamName, boundOpened);
        }
    }
}
