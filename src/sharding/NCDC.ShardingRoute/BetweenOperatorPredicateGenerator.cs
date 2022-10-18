using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.Enums;
using NCDC.Plugin.Enums;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Expressions;
using NCDC.ShardingRoute.Helpers;

namespace NCDC.ShardingRoute;

public sealed class BetweenOperatorPredicateGenerator
{
    private BetweenOperatorPredicateGenerator()
    {
    }

    public static BetweenOperatorPredicateGenerator Instance { get; } = new BetweenOperatorPredicateGenerator();

    public RoutePredicateExpression Get(
        Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter, string columnName,
        BetweenExpression predicateBetweenExpression,
        ParameterContext parameterContext)
    {
        var routePredicateExpression = RoutePredicateExpression.Default;
        IComparable? betweenStartRouteValue = new ConditionValue(predicateBetweenExpression.BetweenExpr, parameterContext).GetValue();
        if (betweenStartRouteValue == null)
        {
            if (ExpressionConditionHelper.IsNowExpression(predicateBetweenExpression.BetweenExpr))
            {
                betweenStartRouteValue = DateTime.Now;
            }
        }

        if (betweenStartRouteValue != null)
        {
            var translateFilter = keyTranslateFilter(betweenStartRouteValue,ShardingOperatorEnum.GREATER_THAN_OR_EQUAL,columnName);
            routePredicateExpression = routePredicateExpression.And(new RoutePredicateExpression(translateFilter));
        }
        IComparable? betweenEndRouteValue = new ConditionValue(predicateBetweenExpression.AndExpr, parameterContext).GetValue();
        if (betweenEndRouteValue == null)
        {
            if (ExpressionConditionHelper.IsNowExpression(predicateBetweenExpression.BetweenExpr))
            {
                betweenEndRouteValue = DateTime.Now;
            }
        }

        if (betweenEndRouteValue != null)
        {
            var translateFilter = keyTranslateFilter(betweenEndRouteValue,ShardingOperatorEnum.LESS_THAN_OR_EQUAL,columnName);
            routePredicateExpression = routePredicateExpression.And(new RoutePredicateExpression(translateFilter));
        }
        
        return routePredicateExpression;
    }
}