namespace NCDC.CommandParser.Segment.DML.Predicate
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 13:55:44
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OrPredicateSegment:ISqlSegment
    {
        private readonly int _startIndex = 0;

        private readonly int _stopIndex = 0;

        private readonly ICollection<AndPredicateSegment> _andPredicates = new LinkedList<AndPredicateSegment>();


        public OrPredicateSegment()
        {
           
        }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<AndPredicateSegment> GetAndPredicates()
        {
            return _andPredicates;
        }
    }
}
