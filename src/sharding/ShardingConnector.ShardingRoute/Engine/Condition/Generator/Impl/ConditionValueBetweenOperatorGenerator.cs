using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.DataStructure.RangeStructure;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.SPI;
using IComparable = System.IComparable;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 12:20:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConditionValueBetweenOperatorGenerator: IConditionValueGenerator<PredicateBetweenRightValue>
    {
        public IRouteValue Generate(PredicateBetweenRightValue predicateRightValue, Column column, List<object> parameters)
        {
            IComparable betweenRouteValue = new ConditionValue(predicateRightValue.BetweenExpression, parameters).GetValue();
            IComparable andRouteValue = new ConditionValue(predicateRightValue.AndExpression, parameters).GetValue();
            if (betweenRouteValue!=null && andRouteValue!=null)
            {
               
                return new RangeRouteValue<IComparable>(column.Name, column.TableName, Range.Closed(betweenRouteValue, andRouteValue));
            }
            var date = SPITimeService.GetInstance().GetTime();
            if (betweenRouteValue==null && ExpressionConditionUtils.IsNowExpression(predicateRightValue.BetweenExpression))
            {
                betweenRouteValue = date;
            }
            if (andRouteValue==null && ExpressionConditionUtils.IsNowExpression(predicateRightValue.AndExpression))
            {
                andRouteValue = date;
            }
            if(betweenRouteValue != null && andRouteValue != null)
            {
                return new RangeRouteValue<IComparable>(column.Name, column.TableName, Range.Closed(betweenRouteValue, andRouteValue));
            }
            return null;
        }

        public IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, List<object> parameters)
        {
            return Generate((PredicateBetweenRightValue) predicateRightValue, column, parameters);
        }
    }
}
