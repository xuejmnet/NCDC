using OpenConnector.Base;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.CommandParserBinder;


public sealed class ConditionValue
{
    private readonly IComparable? _value;

    public ConditionValue(IExpressionSegment expressionSegment, ParameterContext parameterContext)
    {
        _value = GetValue(expressionSegment, parameterContext);
    }

    private IComparable? GetValue(IExpressionSegment expressionSegment, ParameterContext parameterContext)
    {
        if (expressionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
        {
            return GetValue(parameterMarkerExpressionSegment, parameterContext);
        }
        if (expressionSegment is LiteralExpressionSegment literalExpressionSegment)
        {
            return GetValue(literalExpressionSegment);
        }
        return null;
    }

    private IComparable? GetValue(ParameterMarkerExpressionSegment expressionSegment, ParameterContext parameterContext)
    {

        object result = parameterContext.GetParameterValue(expressionSegment.GetParameterName());
        ShardingAssert.Else(result is IComparable, "Sharding value must implements IComparable.");
        return (IComparable)result;
    }

    private IComparable? GetValue(LiteralExpressionSegment expressionSegment)
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
    public IComparable? GetValue()
    {
        return _value;
    }
}