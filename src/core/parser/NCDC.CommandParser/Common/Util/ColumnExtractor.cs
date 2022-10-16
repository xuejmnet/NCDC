using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Util;

public static class ColumnExtractor
{
   
    /**
     * Extract column segment collection.
     *
     * @param expression expression segment
     * @return column segment collection
     */
    public static ICollection<ColumnSegment> Extract( IExpressionSegment expression) {
        ICollection<ColumnSegment> result = new LinkedList<ColumnSegment>();
        if (expression is BinaryOperationExpression binaryOperationExpression) {
            if (binaryOperationExpression.Left is  ColumnSegment leftColumnSegment) {
                result.Add(leftColumnSegment);
            }
            if (binaryOperationExpression.Right is ColumnSegment rightColumnSegment) {
                result.Add(rightColumnSegment);
            }
        }
        if (expression is InExpression inExpression && inExpression.Left is ColumnSegment inColumnSegment) {
            result.Add(inColumnSegment);
        }
        if (expression is BetweenExpression betweenExpression && betweenExpression.Left is ColumnSegment betweenColumnSegment) {
            result.Add(betweenColumnSegment);
        }
        return result;
    }
    
    /**
     * Extract column segments.
     *
     * @param columnSegments column segments
     * @param whereSegments where segments
     */
    public static void ExtractColumnSegments( ICollection<ColumnSegment> columnSegments,  ICollection<WhereSegment> whereSegments) {
        foreach (var whereSegment in whereSegments)
        {
            var andPredicateSegments = ExpressionExtractUtil.GetAndPredicateSegments(whereSegment.Expr);
            foreach (var andPredicateSegment in andPredicateSegments)
            {
                ExtractColumnSegments(columnSegments, andPredicateSegment);
            }
        }
    }
    
    private static void ExtractColumnSegments( ICollection<ColumnSegment> columnSegments,  AndPredicateSegment andPredicateSegment) {
        foreach (var expressionSegment in andPredicateSegment.Predicates)
        {
            columnSegments.AddAll(ColumnExtractor.Extract(expressionSegment));
        }
    } 
}