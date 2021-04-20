namespace ShardingConnector.CommandParser.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:53:37
* @Email: 326308290@qq.com
*/
    public sealed class NumberLiteralLimitValueSegment:LimitValueSegment,INumberLiteralPaginationValueSegment
    {
        private readonly long _value;
        public NumberLiteralLimitValueSegment(int startIndex, int stopIndex,long value) : base(startIndex, stopIndex)
        {
            _value = value;
        }

        public long GetValue()
        {
            return _value;
        }
    }
}