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
        public int StartIndex { get; }
        public int StopIndex { get; }
        public string Text { get; }


        public CommonExpressionSegment(int startIndex, int stopIndex, string text)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Text = text;
        }
    }
}
