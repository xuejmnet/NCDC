using NCDC.CommandParser.Segment.DML.Expr;

namespace NCDC.CommandParser.Segment.DML.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:38:33
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PredicateCompareRightValue:IPredicateRightValue
    {
        private readonly string _operator;

        private readonly IExpressionSegment _expression;

        public PredicateCompareRightValue(string @operator, IExpressionSegment expression)
        {
            _operator = @operator;
            _expression = expression;
        }

        public string GetOperator()
        {
            return _operator;
        }

        public IExpressionSegment GetExpression()
        {
            return _expression;
        }
    }
}
