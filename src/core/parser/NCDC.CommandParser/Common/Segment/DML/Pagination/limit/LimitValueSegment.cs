namespace NCDC.CommandParser.Common.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:52:13
* @Email: 326308290@qq.com
*/
    public abstract class LimitValueSegment:IPaginationValueSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        public LimitValueSegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public bool IsBoundOpened()
        {
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}