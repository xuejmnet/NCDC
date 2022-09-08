namespace OpenConnector.CommandParser.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:11:55
* @Email: 326308290@qq.com
*/
    public sealed  class AndPredicateSegment:ISqlSegment
    {
        private readonly int _startIndex = 0;
    
        private readonly int _stopIndex = 0;
    
        private readonly ICollection<PredicateSegment> _predicates = new LinkedList<PredicateSegment>();
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<PredicateSegment> GetPredicates()
        {
            return _predicates;
        }
    }
}