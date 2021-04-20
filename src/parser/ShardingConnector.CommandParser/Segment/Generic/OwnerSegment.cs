using ShardingConnector.CommandParser.Value.Identifier;

namespace ShardingConnector.CommandParser.Segment.Generic
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly IdentifierValue _identifier;
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public IdentifierValue GetIdentifier()
        {
            return _identifier;
        }
    }
}
