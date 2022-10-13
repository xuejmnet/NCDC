namespace NCDC.CommandParser.Common.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:37:46
* @Email: 326308290@qq.com
*/
    public sealed class LockSegment:ISqlSegment
    {

        public LockSegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public int StartIndex { get; }
        public int StopIndex { get; }
    }
}