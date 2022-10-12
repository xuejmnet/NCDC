using System.Collections.Generic;
using NCDC.CommandParser.Segment.DML.Assignment;

namespace NCDC.CommandParser.Segment.DML.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:40:27
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OnDuplicateKeyColumnsSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ICollection<AssignmentSegment> Columns { get; }


        public OnDuplicateKeyColumnsSegment(int startIndex, int stopIndex, ICollection<AssignmentSegment> columns)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Columns = columns;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Columns)}: {Columns}";
        }
    }
}
