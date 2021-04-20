using ShardingConnector.CommandParser.Constant;

namespace ShardingConnector.CommandParser.Segment.DML.Order.Item
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
        private readonly string _expression;
        public ExpressionOrderByItemSegment(int startIndex, int stopIndex, string expression, OrderDirectionEnum orderDirection) : this(startIndex, stopIndex, expression, orderDirection, OrderDirectionEnum.ASC)
        {
        }
        public ExpressionOrderByItemSegment(int startIndex, int stopIndex,string expression, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection) : base(startIndex, stopIndex, orderDirection, nullOrderDirection)
        {
            _expression = expression;
        }

        public override string GetText()
        {
            return _expression;
        }

        public string GetExpression()
        {
            return _expression;
        }
    }
}
