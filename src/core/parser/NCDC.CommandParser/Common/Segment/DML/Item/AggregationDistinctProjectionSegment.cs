using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Util;

namespace NCDC.CommandParser.Common.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:43:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AggregationDistinctProjectionSegment:AggregationProjectionSegment
    {
       public string DistinctExpression { get; }
        public AggregationDistinctProjectionSegment(int startIndex, int stopIndex, AggregationTypeEnum type,string innerExpression,string distinctExpression) : base(startIndex, stopIndex, type, innerExpression)
        {
            DistinctExpression = SqlUtil.GetExpressionWithoutOutsideParentheses(distinctExpression);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(DistinctExpression)}: {DistinctExpression}";
        }
    }
}
