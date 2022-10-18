using System.Collections.ObjectModel;
using Antlr4.Runtime.Misc;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Util;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Insert.Values;

public sealed class OnDuplicateUpdateContext
{
    private readonly int parameterCount;
    
    private readonly List<IExpressionSegment> valueExpressions;
    
    private readonly ICollection<ParameterMarkerExpressionSegment> parameterMarkerExpressions;
    
    private readonly ParameterContext _parameterContext;
    
    private readonly List<ColumnSegment> columns;
    
    public OnDuplicateUpdateContext( ICollection<AssignmentSegment> assignments,  ParameterContext parameterContext) {
        var expressionSegments = assignments.Select(o=>o.GetValue()).ToList();
        valueExpressions = GetValueExpressions(expressionSegments);
        parameterMarkerExpressions = ExpressionExtractUtil.GetParameterMarkerExpressions(expressionSegments);
        parameterCount = parameterMarkerExpressions.Count;
        _parameterContext = parameterContext;
        columns = assignments.Select(o=>o.GetColumns()[0]).ToList();
    }
    
    private List<IExpressionSegment> GetValueExpressions( List<IExpressionSegment> assignments) {
        return new List<IExpressionSegment>(assignments);
    }
    //
    // /**
    //  * Get value.
    //  *
    //  * @param index index
    //  * @return value
    //  */
    // public Object getValue(readonly int index) {
    //     ExpressionSegment valueExpression = valueExpressions.get(index);
    //     if (valueExpression instanceof ParameterMarkerExpressionSegment) {
    //         return parameters.get(getParameterIndex((ParameterMarkerExpressionSegment) valueExpression));
    //     }
    //     if (valueExpression instanceof FunctionSegment) {
    //         return valueExpression;
    //     }
    //     return ((LiteralExpressionSegment) valueExpression).getLiterals();
    // }
    
    // private int getParameterIndex(readonly ParameterMarkerExpressionSegment parameterMarkerExpression) {
    //     int parameterIndex = parameterMarkerExpressions.indexOf(parameterMarkerExpression);
    //     Preconditions.checkArgument(parameterIndex >= 0, "Can not get parameter index.");
    //     return parameterIndex;
    // }
    
    // /**
    //  * Get on duplicate key update column by index of this clause.
    //  *
    //  * @param index index
    //  * @return columnSegment
    //  */
    // public ColumnSegment getColumn(readonly int index) {
    //     return columns.get(index);
    // }
}