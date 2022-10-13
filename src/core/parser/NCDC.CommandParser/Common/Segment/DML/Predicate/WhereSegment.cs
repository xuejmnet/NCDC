using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:10:53
* @Email: 326308290@qq.com
*/
    public sealed class WhereSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IExpressionSegment Expr { get; }


        public WhereSegment(int startIndex, int stopIndex, IExpressionSegment expr)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Expr = expr;
        }
    }
}