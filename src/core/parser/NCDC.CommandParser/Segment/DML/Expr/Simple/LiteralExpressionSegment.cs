namespace NCDC.CommandParser.Segment.DML.Expr.Simple
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:49:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public sealed class LiteralExpressionSegment:ISimpleExpressionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public object Literals { get; }

        public LiteralExpressionSegment(int startIndex, int stopIndex, object literals)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Literals = literals;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Literals)}: {Literals}";
        }
    }
}
