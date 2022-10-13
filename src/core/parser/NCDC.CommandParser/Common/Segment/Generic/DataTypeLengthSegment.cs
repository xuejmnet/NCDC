namespace NCDC.CommandParser.Common.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:05:13
* @Email: 326308290@qq.com
*/
    public sealed class DataTypeLengthSegment:ISqlSegment
    {
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
        public int Percision { get; set; }
        public int? Scale { get; set; }
    }
}