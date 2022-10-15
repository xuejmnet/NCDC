using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Segment.DAL
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

    }
}