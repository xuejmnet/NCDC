using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Predicate.Value
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
        public  IExpressionSegment BetweenExpression { get; }

        public  IExpressionSegment AndExpression { get; }

        public PredicateBetweenRightValue(IExpressionSegment betweenExpression, IExpressionSegment andExpression)
        {
            BetweenExpression = betweenExpression;
            AndExpression = andExpression;
        }
    }
}
