using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:30:50
* @Email: 326308290@qq.com
*/
    public sealed class FromSchemaSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public DatabaseSegment Schema { get; }

        public FromSchemaSegment(int startIndex, int stopIndex,DatabaseSegment schema)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Schema = schema;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Schema)}: {Schema}";
        }
    }
}