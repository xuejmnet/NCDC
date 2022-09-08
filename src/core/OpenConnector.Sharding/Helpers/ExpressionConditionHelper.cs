using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Complex;
using OpenConnector.Extensions;

namespace OpenConnector.Sharding.Helpers;

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