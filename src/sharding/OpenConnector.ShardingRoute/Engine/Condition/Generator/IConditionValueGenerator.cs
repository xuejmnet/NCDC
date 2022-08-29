using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.ShardingAdoNet;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace OpenConnector.ShardingRoute.Engine.Condition.Generator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 12:16:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IConditionValueGenerator
    {
        IRouteValue Generate(IPredicateRightValue predicateRightValue, Column column, ParameterContext parameterContext);
    }
    public interface IConditionValueGenerator<in T>: IConditionValueGenerator where T : IPredicateRightValue
    {
        IRouteValue Generate(T predicateRightValue, Column column, ParameterContext parameterContext);

    }
}
