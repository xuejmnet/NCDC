using System;
using ShardingConnector.Parser.Sql.Value.Identifier;

namespace ShardingConnector.Parser.Sql.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:03:15
* @Email: 326308290@qq.com
*/
    public sealed class SchemaSegment:ISqlSegment
    {
        
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly IdentifierValue _identifier;

        public SchemaSegment(int startIndex, int stopIndex, IdentifierValue identifier)
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

        public IdentifierValue GetIdentifierValue()
        {
            return _identifier;
        }
    }
}