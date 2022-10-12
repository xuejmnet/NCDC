using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Segment.DDL.Constraint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:10:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DropPrimaryKeySegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        public DropPrimaryKeySegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}
