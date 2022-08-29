using OpenConnector.CommandParser.Segment.Generic;

namespace OpenConnector.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:30:50
* @Email: 326308290@qq.com
*/
    public sealed class FromSchemaSegment:ISqlSegment,IRemoveAvailable
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;

        public FromSchemaSegment(int startIndex, int stopIndex)
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