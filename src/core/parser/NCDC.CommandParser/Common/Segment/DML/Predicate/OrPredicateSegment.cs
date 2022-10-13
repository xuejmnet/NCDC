namespace NCDC.CommandParser.Common.Segment.DML.Predicate
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

        public int StartIndex => 0;
        public int StopIndex => 0;
        public  ICollection<AndPredicateSegment> AndPredicates = new LinkedList<AndPredicateSegment>();

    }
}
