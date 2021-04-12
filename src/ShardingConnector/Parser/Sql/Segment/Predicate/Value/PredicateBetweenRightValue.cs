using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Parser.Sql.Segment.DML.Expr;

namespace ShardingConnector.Parser.Sql.Segment.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:39:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    ///  Predicate right value for BETWEEN ... AND ... operator.
    /// </summary>
    public sealed class PredicateBetweenRightValue:IPredicateRightValue
    {
        private readonly IExpressionSegment _betweenExpression;
    
        private readonly IExpressionSegment _andExpression;

        public PredicateBetweenRightValue(IExpressionSegment betweenExpression, IExpressionSegment andExpression)
        {
            _betweenExpression = betweenExpression;
            _andExpression = andExpression;
        }
    }
}
