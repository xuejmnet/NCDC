using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.Generic.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:25:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableNameSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IdentifierValue IdentifierValue { get; }

        public TableNameSegment(int startIndex, int stopIndex, IdentifierValue identifierValue)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IdentifierValue = identifierValue;
        }

    }
}
