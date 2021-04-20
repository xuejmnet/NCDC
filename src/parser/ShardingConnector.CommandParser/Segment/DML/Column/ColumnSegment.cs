using ShardingConnector.CommandParser.Segment.Generic;
using ShardingConnector.CommandParser.Segment.Predicate.Value;
using ShardingConnector.CommandParser.Value.Identifier;

namespace ShardingConnector.CommandParser.Segment.DML.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 15:17:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnSegment:ISqlSegment,IPredicateRightValue,IOwnerAvailable
    {
        
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly IdentifierValue _identifier;
        private  OwnerSegment owner;

        public ColumnSegment(int startIndex, int stopIndex, IdentifierValue identifier)
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

        public OwnerSegment GetOwner()
        {
            return owner;
        }

        public void SetOwner(OwnerSegment owner)
        {
            this.owner = owner;
        }
        /// <summary>
        /// 获取所属值如table.column
        /// </summary>
        /// <returns></returns>
        public string GetQualifiedName() {
            return null == owner ? _identifier.GetValue() : owner.GetIdentifier().GetValue() + "." + _identifier.GetValue();
        }
    }
}
