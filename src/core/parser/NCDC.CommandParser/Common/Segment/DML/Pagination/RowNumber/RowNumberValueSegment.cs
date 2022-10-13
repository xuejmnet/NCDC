namespace NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber
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

        private readonly bool _boundOpened;
        public int StartIndex { get; }
        public int StopIndex { get; }

        protected RowNumberValueSegment(int startIndex, int stopIndex, bool boundOpened)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _boundOpened = boundOpened;
        }

        public bool IsBoundOpened()
        {
            return _boundOpened;
        }

    }
}
