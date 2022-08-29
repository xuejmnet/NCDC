using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Complex;
using OpenConnector.Extensions;

namespace OpenConnector.ShardingRoute.Engine.Condition
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 12:10:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ExpressionConditionUtils
    {
        /**
         * Judge now() expression.
         * @param segment ExpressionSegment
         * @return true or false
         */
        public static bool IsNowExpression( IExpressionSegment segment)
        {
            return segment is IComplexExpressionSegment complexExpressionSegment && "now()".EqualsIgnoreCase(complexExpressionSegment.GetText());
        }

        /**
         * Judge null expression.
         * @param segment ExpressionSegment
         * @return true or false
         */
        public static bool IsNullExpression( IExpressionSegment segment)
        {
            return segment is CommonExpressionSegment commonExpressionSegment && "null".EqualsIgnoreCase(commonExpressionSegment.GetText());
        }
    }
}
