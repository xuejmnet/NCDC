using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:04:27
* @Email: 326308290@qq.com
*/
    public sealed class DataTypeSegment:IExpressionSegment
    {
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
    
        public string? DataTypeName { get; set; }
    
        public DataTypeLengthSegment? DataLength { get; set; }
    }
}