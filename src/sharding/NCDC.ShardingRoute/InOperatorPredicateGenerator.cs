using NCDC.CommandParser.Segment.DML.Predicate.Value;
using NCDC.Enums;
using NCDC.Plugin.Enums;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Expressions;
using NCDC.ShardingRoute.Helpers;

namespace NCDC.ShardingRoute;

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