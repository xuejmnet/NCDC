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

        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly ICollection<AssignmentSegment> _columns;



        public OnDuplicateKeyColumnsSegment(int startIndex, int stopIndex, ICollection<AssignmentSegment> columns)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _columns = columns;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<AssignmentSegment> GetColumns()
        {
            return _columns;
        }
    }
}
