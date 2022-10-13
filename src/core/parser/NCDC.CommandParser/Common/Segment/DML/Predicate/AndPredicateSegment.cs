using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Predicate
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:11:55
* @Email: 326308290@qq.com
*/
    public sealed  class AndPredicateSegment:ISqlSegment
    {
        public int StartIndex => 0;
        public int StopIndex => 0;
    
        public  ICollection<IExpressionSegment> Predicates = new LinkedList<IExpressionSegment>();

    }
}