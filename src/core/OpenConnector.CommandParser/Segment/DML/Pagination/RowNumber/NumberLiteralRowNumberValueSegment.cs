namespace OpenConnector.CommandParser.Segment.DML.Pagination.RowNumber
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:23:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class NumberLiteralRowNumberValueSegment:RowNumberValueSegment,INumberLiteralPaginationValueSegment
    {
        private readonly long _value;
        public NumberLiteralRowNumberValueSegment(int startIndex, int stopIndex,long value, bool boundOpened) : base(startIndex, stopIndex, boundOpened)
        {
            _value = value;
        }

        public long GetValue()
        {
            return _value;
        }
    }
}
