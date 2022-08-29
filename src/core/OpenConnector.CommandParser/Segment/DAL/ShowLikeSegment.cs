namespace OpenConnector.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:27:32
* @Email: 326308290@qq.com
*/
    public sealed class ShowLikeSegment:ISqlSegment
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly string _pattern;

        public ShowLikeSegment(int startIndex, int stopIndex, string pattern)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _pattern = pattern;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string GetPattern()
        {
            return _pattern;
        }
    }
}