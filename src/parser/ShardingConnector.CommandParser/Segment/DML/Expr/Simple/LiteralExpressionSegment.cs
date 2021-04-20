namespace ShardingConnector.CommandParser.Segment.DML.Expr.Simple
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:49:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public class LiteralExpressionSegment:ISimpleExpressionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly object _literals;

        public LiteralExpressionSegment(int startIndex, int stopIndex, object literals)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _literals = literals;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public object GetLiterals()
        {
            return _literals;
        }
    }
}
