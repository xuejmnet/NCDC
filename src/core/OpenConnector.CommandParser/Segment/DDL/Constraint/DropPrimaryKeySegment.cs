using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.CommandParser.Segment.DDL.Constraint
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public DropPrimaryKeySegment(int startIndex, int stopIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }
    }
}
