using OpenConnector.CommandParser.Value.Identifier;

namespace OpenConnector.CommandParser.Segment.Generic.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:25:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableNameSegment:ISqlSegment
    {
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly IdentifierValue _identifier;

        public TableNameSegment(int startIndex, int stopIndex, IdentifierValue identifier)
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
