using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Order.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 13:36:52
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ExpressionOrderByItemSegment:TextOrderByItemSegment
    {
        private readonly IExpressionSegment? _expr;
        private readonly string _expression;
        public ExpressionOrderByItemSegment(int startIndex, int stopIndex, string expression, OrderDirectionEnum orderDirection) : this(startIndex, stopIndex, expression, orderDirection, OrderDirectionEnum.ASC)
        {
        }
        public ExpressionOrderByItemSegment(int startIndex, int stopIndex, string expression, OrderDirectionEnum orderDirection,IExpressionSegment expr) : this(startIndex, stopIndex, expression, orderDirection, OrderDirectionEnum.ASC)
        {
            _expr = expr;
        }
        public ExpressionOrderByItemSegment(int startIndex, int stopIndex,string expression, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection) : base(startIndex, stopIndex, orderDirection, nullOrderDirection)
        {
            _expression = expression;
        }

        public override string GetExpression()
        {
            return _expression;
        }
        public  IExpressionSegment? GetExpr()
        {
            return _expr;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Expr: {_expr}, Expression: {_expression}";
        }
    }
}
