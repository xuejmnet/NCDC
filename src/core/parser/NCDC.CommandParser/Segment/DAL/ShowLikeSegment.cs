namespace NCDC.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:27:32
* @Email: 326308290@qq.com
*/
    public sealed class ShowLikeSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public string Pattern { get; }

        public ShowLikeSegment(int startIndex, int stopIndex, string pattern)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Pattern = pattern;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Pattern)}: {Pattern}";
        }
    }
}