using System.Collections.Generic;
using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.CommandParser.Segment.Predicate;

namespace ShardingConnector.CommandParser.Segment.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:30:21
* @Email: 326308290@qq.com
*/
    public sealed class JoinSpecificationSegment:ISqlSegment
    {
        
        private int _startIndex;
    
        private int _stopIndex;
    
        private PredicateSegment _predicateSegment;
    
        private readonly ICollection<ColumnSegment> _usingColumns = new LinkedList<ColumnSegment>();
        
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public void SetStartIndex(int startIndex)
        {
            this._startIndex = startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public void SetStopIndex(int stopIndex)
        {
            this._stopIndex = stopIndex;
        }

        public PredicateSegment GetPredicateSegment()
        {
            return _predicateSegment;
        }

        public void SetPredicateSegment(PredicateSegment predicate)
        {
            this._predicateSegment = predicate;
        }

        public ICollection<ColumnSegment> GetUsingColumn()
        {
            return this._usingColumns;
        }
    }
}