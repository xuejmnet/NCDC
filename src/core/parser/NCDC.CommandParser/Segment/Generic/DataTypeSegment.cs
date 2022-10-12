namespace NCDC.CommandParser.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:04:27
* @Email: 326308290@qq.com
*/
    public sealed class DataTypeSegment:ISqlSegment
    {
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
    
        public string? DataTypeName { get; set; }
    
        public DataTypeLengthSegment? DataLength { get; set; }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(DataTypeName)}: {DataTypeName}, {nameof(DataLength)}: {DataLength}";
        }
    }
}