namespace NCDC.CommandParser.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:37:46
* @Email: 326308290@qq.com
*/
    public sealed class LockSegment:ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public LockSegment(int startIndex, int stopIndex)
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
    }
}