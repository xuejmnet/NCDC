using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;

namespace ShardingConnector.CommandParser.Segment.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:31:35
* @Email: 326308290@qq.com
*/
    public class PredicateSegment:ISqlSegment
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly ColumnSegment _column;
    
        private readonly IPredicateRightValue _rightValue;

        public PredicateSegment(int startIndex, int stopIndex, ColumnSegment column, IPredicateRightValue rightValue)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _column = column;
            _rightValue = rightValue;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ColumnSegment GetColumn()
        {
            return _column;
        }

        public IPredicateRightValue GetPredicateRightValue()
        {
            return _rightValue;
        }
    }
}