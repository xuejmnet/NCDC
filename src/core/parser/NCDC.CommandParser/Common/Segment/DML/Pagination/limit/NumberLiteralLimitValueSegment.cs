namespace NCDC.CommandParser.Common.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:53:37
* @Email: 326308290@qq.com
*/
    public sealed class NumberLiteralLimitValueSegment:LimitValueSegment,INumberLiteralPaginationValueSegment
    {
        public NumberLiteralLimitValueSegment(int startIndex, int stopIndex,long value) : base(startIndex, stopIndex)
        {
            Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, {base.ToString()}, {nameof(Value)}: {Value}";
        }
    }
}