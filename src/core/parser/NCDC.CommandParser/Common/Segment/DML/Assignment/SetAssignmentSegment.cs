using System.Collections.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Assignment
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:50:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SetAssignmentSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ICollection<AssignmentSegment> Assignments { get; }

        public SetAssignmentSegment(int startIndex, int stopIndex, ICollection<AssignmentSegment> assignments)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Assignments = assignments;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Assignments)}: {Assignments}";
        }
    }
}
