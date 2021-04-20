namespace ShardingConnector.CommandParser.Segment.DML.Pagination.RowNumber
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:21:30
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class RowNumberValueSegment:IPaginationValueSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly bool _boundOpened;

        protected RowNumberValueSegment(int startIndex, int stopIndex, bool boundOpened)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _boundOpened = boundOpened;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public bool isBoundOpened()
        {
            return _boundOpened;
        }
    }
}
