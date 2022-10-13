namespace NCDC.CommandParser.Common.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:33:20
* @Email: 326308290@qq.com
*/
    public sealed class LimitSegment : ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IPaginationValueSegment Offset { get; }
        public IPaginationValueSegment RowCount { get; }


        public LimitSegment(int startIndex, int stopIndex, IPaginationValueSegment offset, IPaginationValueSegment rowCount)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Offset = offset;
            RowCount = rowCount;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Offset)}: {Offset}, {nameof(RowCount)}: {RowCount}";
        }
    }
}