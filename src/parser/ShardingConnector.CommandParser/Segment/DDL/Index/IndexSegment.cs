using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Value.Identifier;

namespace ShardingConnector.CommandParser.Segment.DDL.Index
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:11:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IndexSegment:ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public IndexSegment(int startIndex, int stopIndex, IdentifierValue identifier)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            Identifier = identifier;
        }

        public  IdentifierValue Identifier { get; }
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
