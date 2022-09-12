using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Complex;
using NCDC.Extensions;

namespace NCDC.ShardingRoute.Helpers;

public static class ExpressionConditionHelper
{
    public static bool IsNowExpression(IExpressionSegment segment)
    {
        return segment is IComplexExpressionSegment complexExpressionSegment &&
               "now()".EqualsIgnoreCase(complexExpressionSegment.GetText());
    }

    public static bool IsNullExpression(IExpressionSegment segment)
    {
        return segment is CommonExpressionSegment commonExpressionSegment &&
               "null".EqualsIgnoreCase(commonExpressionSegment.GetText());
    }
}