using System.Collections.Generic;

namespace ShardingConnector.CommandParser.Segment.DML.Assignment
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
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly ICollection<AssignmentSegment> _assignments;

        public SetAssignmentSegment(int startIndex, int stopIndex, ICollection<AssignmentSegment> assignments)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _assignments = assignments;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<AssignmentSegment> GetAssignments()
        {
            return _assignments;
        }
    }
}
