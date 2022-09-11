namespace NCDC.CommandParser.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:10:53
* @Email: 326308290@qq.com
*/
    public sealed class WhereSegment:ISqlSegment
    {
        private readonly int startIndex;
    
        private readonly int stopIndex;
    
        private readonly ICollection<AndPredicateSegment> _andPredicates = new LinkedList<AndPredicateSegment>();

        public WhereSegment(int startIndex, int stopIndex)
        {
            this.startIndex = startIndex;
            this.stopIndex = stopIndex;
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public int GetStopIndex()
        {
            return stopIndex;
        }

        public ICollection<AndPredicateSegment> GetAndPredicates()
        {
            return _andPredicates;
        }
    }
}