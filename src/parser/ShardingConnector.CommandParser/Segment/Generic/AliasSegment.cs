using ShardingConnector.CommandParser.Value.Identifier;

namespace ShardingConnector.CommandParser.Segment.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:07:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 数据库别名片断
    /// </summary>
    public sealed class AliasSegment:ISqlSegment
    {
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly IdentifierValue _identifier;

        public AliasSegment(int startIndex, int stopIndex, IdentifierValue identifier)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _identifier = identifier;
        }

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
