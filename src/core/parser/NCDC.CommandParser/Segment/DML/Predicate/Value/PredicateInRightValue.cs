using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Expr;

namespace NCDC.CommandParser.Segment.DML.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:34:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PredicateInRightValue:IPredicateRightValue
    {
        public PredicateInRightValue(PredicateBracketValue predicateBracketValue, ICollection<IExpressionSegment> sqlExpressions)
        {
            PredicateBracketValue = predicateBracketValue;
            SqlExpressions = sqlExpressions;
        }

        public PredicateBracketValue PredicateBracketValue{get;}

        public ICollection<IExpressionSegment> SqlExpressions { get; }
    }
}
