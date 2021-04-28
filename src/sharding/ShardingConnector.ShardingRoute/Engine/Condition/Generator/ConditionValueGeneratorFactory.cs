using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using ShardingConnector.ShardingRoute.Engine.Condition.Generator.Impl;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:30:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ConditionValueGeneratorFactory
    {
        private ConditionValueGeneratorFactory()
        {
            
        }
        public static IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, List<object> parameters)
        {
            if (predicateRightValue is PredicateCompareRightValue predicateCompareRightValue) {
                return new ConditionValueCompareOperatorGenerator().Generate(predicateCompareRightValue, column, parameters);
            }
            if (predicateRightValue is PredicateInRightValue predicateInRightValue) {
                return new ConditionValueInOperatorGenerator().Generate(predicateInRightValue, column, parameters);
            }
            if (predicateRightValue is PredicateBetweenRightValue predicateBetweenRightValue) {
                return new ConditionValueBetweenOperatorGenerator().Generate(predicateBetweenRightValue, column, parameters);
            }
            return null;
        }
    }
}
