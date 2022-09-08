using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Predicate;

namespace OpenConnector.CommandParser.Segment.DML
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
    
        private  ICollection<ColumnSegment> usingColumns = new LinkedList<ColumnSegment>();
        
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

        public ICollection<ColumnSegment> GetUsingColumns()
        {
            return this.usingColumns;
        }

        public void SetUsingColumns(ICollection<ColumnSegment> usingColumns)
        {
            this.usingColumns = usingColumns;
        }
    }
}