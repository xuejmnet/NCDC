using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.SPI;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:27:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConditionValueInOperatorGenerator: IConditionValueGenerator<PredicateInRightValue>
    {
        public IRouteValue Generate(PredicateInRightValue predicateRightValue, Column column, List<object> parameters)
        {
            ICollection<IComparable> routeValues = new LinkedList<IComparable>();
            SPITimeService timeService = SPITimeService.GetInstance();
            foreach (var sqlExpression in predicateRightValue.SqlExpressions)
            {
                var routeValue = new ConditionValue(sqlExpression, parameters).GetValue();
                if (routeValue!=null)
                {
                    routeValues.Add(routeValue);
                    continue;
                }
                if (ExpressionConditionUtils.IsNowExpression(sqlExpression))
                {
                    routeValues.Add(timeService.GetTime());
                }
            }
            return routeValues.IsEmpty() ? null : new ListRouteValue<IComparable>(column.Name, column.TableName, routeValues);

        }

        public IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, List<object> parameters)
        {
            return Generate((PredicateInRightValue) predicateRightValue, column, parameters);
        }
    }
}
