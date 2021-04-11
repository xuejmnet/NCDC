using System;

namespace ShardingConnector.Parser.Sql.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:33:20
* @Email: 326308290@qq.com
*/
    public sealed class LimitSegment : ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly IPaginationValueSegment _offset;

        private readonly IPaginationValueSegment _rowCount;

        public LimitSegment(int startIndex, int stopIndex, IPaginationValueSegment offset, IPaginationValueSegment rowCount)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _offset = offset;
            _rowCount = rowCount;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        /**
     * Get offset.
     * 
     * @return offset
     */
        public IPaginationValueSegment GetOffset()
        {
            return _offset;
        }

        /**
     * Get row count.
     *
     * @return row count
     */
        public IPaginationValueSegment GetRowCount()
        {
            return _rowCount;
        }
    }
}