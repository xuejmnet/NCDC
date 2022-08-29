namespace OpenConnector.CommandParser.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:52:13
* @Email: 326308290@qq.com
*/
    public abstract class LimitValueSegment:IPaginationValueSegment
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;

        public LimitValueSegment(int startIndex, int stopIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
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
            return false;
        }
    }
}