using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Extensions;
using ShardingConnector.CommandParser.Segment.DML.Expr;
using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Generator
{
    /*
    * @Author: xjm
    * @Description: 1
    * @Date: 2021/4/28 12:13:09
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConditionValue
    {
        private readonly IComparable _value;

        public ConditionValue(IExpressionSegment expressionSegment, List<object> parameters)
        {
            _value = GetValue(expressionSegment, parameters);
        }

        private IComparable GetValue(IExpressionSegment expressionSegment, List<object> parameters)
        {
            if (expressionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                return GetValue(parameterMarkerExpressionSegment, parameters);
            }
            if (expressionSegment is LiteralExpressionSegment literalExpressionSegment)
            {
                return GetValue(literalExpressionSegment);
            }
            return null;
        }

        private IComparable GetValue(ParameterMarkerExpressionSegment expressionSegment, List<object> parameters)
        {

            object result = parameters.GetParameterValue(expressionSegment);
            ShardingAssert.Else(result is IComparable, "Sharding value must implements IComparable.");
            return (IComparable)result;
        }

        private IComparable GetValue(LiteralExpressionSegment expressionSegment)
        {
            object result = expressionSegment.GetLiterals();
            ShardingAssert.Else(result is IComparable, "Sharding value must implements IComparable.");
            return (IComparable)result;
        }

        /**
         * Get condition value.
         * 
         * @return condition value
         */
        public IComparable GetValue()
        {
            return _value;
        }
    }
}
