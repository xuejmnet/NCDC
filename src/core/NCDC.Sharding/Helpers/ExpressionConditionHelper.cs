using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Complex;
using OpenConnector.Extensions;

namespace NCDC.Sharding.Helpers;

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