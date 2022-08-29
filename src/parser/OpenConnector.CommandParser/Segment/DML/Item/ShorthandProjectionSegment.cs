using OpenConnector.CommandParser.Segment.Generic;

namespace OpenConnector.CommandParser.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 21:55:09
* @Email: 326308290@qq.com
*/
    public sealed class ShorthandProjectionSegment:IProjectionSegment,IOwnerAvailable
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private OwnerSegment owner;

        public ShorthandProjectionSegment(int startIndex, int stopIndex)
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

        public OwnerSegment GetOwner()
        {
            return owner;
        }

        public void SetOwner(OwnerSegment owner)
        {
            this.owner = owner;
        }
    }
}