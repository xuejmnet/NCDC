using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.DataStructure.RangeStructure;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.SPI;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 13:57:04
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConditionValueCompareOperatorGenerator : IConditionValueGenerator<PredicateCompareRightValue>
    {
        private  const string EQUAL = "=";

        private const string GREATER_THAN = ">";

        private const string LESS_THAN = "<";

        private const string AT_MOST = "<=";

        private const string AT_LEAST = ">=";

        private static readonly List<string> OPERATORS = new List<string>() { EQUAL, GREATER_THAN, LESS_THAN, AT_LEAST, AT_MOST };
        public IRouteValue Generate(PredicateCompareRightValue predicateRightValue, Column column, List<object> parameters)
        {
            string @operator = predicateRightValue.GetOperator();
            if (!IsSupportedOperator(@operator))
            {
                return null;
            }
            IComparable routeValue = new ConditionValue(predicateRightValue.GetExpression(), parameters).GetValue();
            if (routeValue != null)
            {
                return Generate(routeValue, column, @operator);
            }
            if (ExpressionConditionUtils.IsNowExpression(predicateRightValue.GetExpression()))
            {
                return Generate(SPITimeService.GetInstance().GetTime(), column, @operator);
            }
            return null;
        }

        private IRouteValue Generate(IComparable comparable, Column column, string @operator)
        {
            string columnName = column.Name;
            string tableName = column.TableName;
            switch (@operator)
            {
                case EQUAL:
                    return new ListRouteValue(columnName, tableName, new List<IComparable>() { comparable });
                case GREATER_THAN:
                    return new RangeRouteValue(columnName, tableName, Range.GreaterThan(comparable));
                case LESS_THAN:
                    return new RangeRouteValue(columnName, tableName, Range.LessThan(comparable));
                case AT_MOST:
                    return new RangeRouteValue(columnName, tableName, Range.AtMost(comparable));
                case AT_LEAST:
                    return new RangeRouteValue(columnName, tableName, Range.AtLeast(comparable));
                default:
                    return null;
            }
        }

        private bool IsSupportedOperator(string @operator)
        {
            return OPERATORS.Contains(@operator);
        }

        public IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, List<object> parameters)
        {
            return Generate((PredicateCompareRightValue)predicateRightValue, column, parameters);
        }
    }
}
