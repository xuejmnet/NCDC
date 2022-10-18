using Antlr4.Runtime.Misc;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Util;

public static class ExpressionExtractUtil
{
    
    /**
     * Get and predicate collection.
     * 
     * @param expression expression segment
     * @return and predicate collection
     */
    public static ICollection<AndPredicateSegment> GetAndPredicateSegments( IExpressionSegment expression) {
        ICollection<AndPredicateSegment> result = new LinkedList<AndPredicateSegment>();
        ExtractAndPredicateSegments(result, expression);
        return result;
    }
    
    private static void ExtractAndPredicateSegments( ICollection<AndPredicateSegment> result,  IExpressionSegment expression) {
        if (!(expression is BinaryOperationExpression binaryOperationExpression)) {
            result.Add(CreateAndPredicateSegment(expression));
            return;
        }
        var logicalOperator = LogicalOperator.ValueFrom(binaryOperationExpression.Operator);
        if (logicalOperator is LogicalOperatorEnum.OR) {
            ExtractAndPredicateSegments(result, binaryOperationExpression.Left);
            ExtractAndPredicateSegments(result, binaryOperationExpression.Right);
        } else if (logicalOperator is LogicalOperatorEnum.AND) {
            ICollection<AndPredicateSegment> predicates = GetAndPredicateSegments(binaryOperationExpression.Right);
            var andPredicateSegments = GetAndPredicateSegments(binaryOperationExpression.Left);
            foreach (var andPredicateSegment in andPredicateSegments)
            {
                ExtractCombinedAndPredicateSegments(result, andPredicateSegment, predicates);
            }
        } else {
            result.Add(CreateAndPredicateSegment(expression));
        }
    }
    
    private static void ExtractCombinedAndPredicateSegments( ICollection<AndPredicateSegment> result,  AndPredicateSegment current,  ICollection<AndPredicateSegment> predicates) {
        foreach (var andPredicateSegment in predicates)
        {
            AndPredicateSegment predicate = new AndPredicateSegment();
            predicate.Predicates.AddAll(current.Predicates);
            predicate.Predicates.AddAll(andPredicateSegment.Predicates);
            result.Add(predicate);
        }
    }
    
    private static AndPredicateSegment CreateAndPredicateSegment( IExpressionSegment expression) {
        AndPredicateSegment result = new AndPredicateSegment();
        result.Predicates.Add(expression);
        return result;
    }
    
    /**
     * Get parameter marker expression collection.
     * 
     * @param expressions expression collection
     * @return parameter marker expression collection
     */
    public static ICollection<ParameterMarkerExpressionSegment> GetParameterMarkerExpressions( IEnumerable<IExpressionSegment> expressions)
    {
        ICollection<ParameterMarkerExpressionSegment> result = new LinkedList<ParameterMarkerExpressionSegment>();
        ExtractParameterMarkerExpressions(result, expressions);
        return result;
    }
    
    private static void ExtractParameterMarkerExpressions( ICollection<ParameterMarkerExpressionSegment> result,  IEnumerable<IExpressionSegment> expressions) {
        foreach (var expressionSegment in expressions)
        {
            if (expressionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment) {
                result.Add(parameterMarkerExpressionSegment);
            }
            // TODO support more expression type if necessary
            if (expressionSegment is BinaryOperationExpression binaryOperationExpression) {
                ExtractParameterMarkerExpressions(result, new[]{binaryOperationExpression.Left});
                ExtractParameterMarkerExpressions(result, new[]{binaryOperationExpression.Right});
            }
            if (expressionSegment is FunctionSegment functionSegment) {
                ExtractParameterMarkerExpressions(result, functionSegment.Parameters);
            }
            
        }
    }
}