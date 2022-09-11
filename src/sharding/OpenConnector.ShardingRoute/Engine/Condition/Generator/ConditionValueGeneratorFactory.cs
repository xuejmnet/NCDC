using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using NCDC.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.ShardingAdoNet;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;
using OpenConnector.ShardingRoute.Engine.Condition.Generator.Impl;

namespace OpenConnector.ShardingRoute.Engine.Condition.Generator
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
        private static readonly ConditionValueCompareOperatorGenerator _conditionValueCompareOperatorGenerator =
            new ConditionValueCompareOperatorGenerator();
        private static readonly ConditionValueInOperatorGenerator _conditionValueInOperatorGenerator =
            new ConditionValueInOperatorGenerator();
        private static readonly ConditionValueBetweenOperatorGenerator _conditionValueBetweenOperatorGenerator =
            new ConditionValueBetweenOperatorGenerator();
        private ConditionValueGeneratorFactory()
        {
            
        }
        public static IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, ParameterContext parameterContext)
        {
            if (predicateRightValue is PredicateCompareRightValue predicateCompareRightValue) {
                return _conditionValueCompareOperatorGenerator.Generate(predicateCompareRightValue, column, parameterContext);
            }
            if (predicateRightValue is PredicateInRightValue predicateInRightValue) {
                return _conditionValueInOperatorGenerator.Generate(predicateInRightValue, column, parameterContext);
            }
            if (predicateRightValue is PredicateBetweenRightValue predicateBetweenRightValue) {
                return _conditionValueBetweenOperatorGenerator.Generate(predicateBetweenRightValue, column, parameterContext);
            }
            return null;
        }
    }
}
