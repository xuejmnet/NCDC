using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DDL.Index
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:11:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IndexSegment:ISqlSegment,IOwnerAvailable
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public OwnerSegment? Owner { get; set; }
        public  IdentifierValue IdentifierValue { get; }

        public IndexSegment(int startIndex, int stopIndex, IdentifierValue identifierValue)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IdentifierValue = identifierValue;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Owner)}: {Owner}, {nameof(IdentifierValue)}: {IdentifierValue}";
        }
    }
}
