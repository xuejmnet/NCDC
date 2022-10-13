using Microsoft.Extensions.Logging;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Expressions;
using NCDC.ShardingRoute.Helpers;
using NCDC.Logger;
using NCDC.Plugin.Enums;

namespace NCDC.ShardingRoute;

public sealed class CompareOperatorPredicateGenerator
{
    private static readonly ILogger<CompareOperatorPredicateGenerator> _logger =
        NCDCLoggerFactory.CreateLogger<CompareOperatorPredicateGenerator>();
    
    private  const string EQUAL = "=";
    private const string GREATER_THAN = ">";
    private const string GREATER_THAN_OR_EQUAL = ">=";

    private const string LESS_THAN = "<";

    private const string LESS_THAN_OR_EQUAL = "<=";


    private static readonly Func<string, ShardingOperatorEnum> ShardingOperatorFunc = @operator =>
    {
        switch (@operator)
        {
            case EQUAL: return ShardingOperatorEnum.EQUAL;
            case GREATER_THAN: return ShardingOperatorEnum.GREATER_THAN;
            case GREATER_THAN_OR_EQUAL: return ShardingOperatorEnum.GREATER_THAN_OR_EQUAL;
            case LESS_THAN: return ShardingOperatorEnum.LESS_THAN;
            case LESS_THAN_OR_EQUAL: return ShardingOperatorEnum.LESS_THAN_OR_EQUAL;
            default: return ShardingOperatorEnum.UN_KNOWN;
        }
    };

    private CompareOperatorPredicateGenerator()
    {
    }

    public static CompareOperatorPredicateGenerator Instance { get; } = new CompareOperatorPredicateGenerator();

    public RoutePredicateExpression Get(Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter,string columnName,PredicateCompareRightValue predicateRightValue,
        ParameterContext parameterContext)
    {
        string @operator = predicateRightValue.GetOperator();
        var shardingOperator = ShardingOperatorFunc(@operator);
        if (IsNotSupportedOperator(shardingOperator))
        {
            _logger.LogWarning($"not support {@operator}");
            throw new ShardingInvalidOperationException($"not support {@operator}");
        }
        
        IComparable? routeValue = new ConditionValue(predicateRightValue.GetExpression(), parameterContext).GetValue();
        if (routeValue == null)
        {
            if (ExpressionConditionHelper.IsNowExpression(predicateRightValue.GetExpression()))
            {
                routeValue = DateTime.Now;
            }
        }

        if (routeValue != null)
        {
            var translateFilter = keyTranslateFilter(routeValue,shardingOperator,columnName);
            return new RoutePredicateExpression(translateFilter);
        }
        throw new ShardingInvalidOperationException($"sharding value cant null");
        
    }
    private bool IsNotSupportedOperator(ShardingOperatorEnum shardingOperator)
    {
        return ShardingOperatorEnum.UN_KNOWN==shardingOperator;
    }
}