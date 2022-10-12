using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:19:30
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OwnerSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IdentifierValue IdentifierValue { get; }

        public OwnerSegment(int startIndex, int stopIndex, IdentifierValue identifierValue)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IdentifierValue = identifierValue;
        }
        public OwnerSegment? Owner { get; set; }

    }
}
