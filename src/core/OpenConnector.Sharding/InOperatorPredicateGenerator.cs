using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.CommandParserBinder;
using OpenConnector.Exceptions;
using OpenConnector.Sharding.Expressions;
using OpenConnector.Sharding.Helpers;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding;

public sealed class InOperatorPredicateGenerator
{
    
    private InOperatorPredicateGenerator()
    {
    }

    public static InOperatorPredicateGenerator Instance { get; } = new InOperatorPredicateGenerator();

    
    public RoutePredicateExpression Get(Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter,string columnName,PredicateInRightValue predicateRightValue,
        ParameterContext parameterContext)
    {
        var contains = RoutePredicateExpression.DefaultFalse;
        foreach (var sqlExpression in predicateRightValue.SqlExpressions)
        {
            var routeValue = new ConditionValue(sqlExpression, parameterContext).GetValue();
            if (routeValue==null)
            {
                if (ExpressionConditionHelper.IsNowExpression(sqlExpression))
                {
                    routeValue = DateTime.Now;
                }
            }

            if (routeValue != null)
            {
                var eq = keyTranslateFilter(routeValue,ShardingOperatorEnum.EQUAL,columnName);
                contains=contains.Or(new RoutePredicateExpression(eq));
            }
        }

        return contains;
    }
}