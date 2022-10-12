using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:28:55
* @Email: 326308290@qq.com
*/
    public sealed class FromTableSegment:ISqlSegment
    {
        public int StartIndex { get; set;}
    
        public int StopIndex { get; set;}
    
        public SimpleTableSegment? Table { get; set;}

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Table)}: {Table}";
        }
    }
}