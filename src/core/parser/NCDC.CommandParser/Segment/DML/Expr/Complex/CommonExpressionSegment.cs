namespace NCDC.CommandParser.Segment.DML.Expr.Complex
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:12:40
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class CommonExpressionSegment:IComplexExpressionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly string _text;

        public CommonExpressionSegment(int startIndex, int stopIndex, string text)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _text = text;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string GetText()
        {
            return _text;
        }
    }
}
