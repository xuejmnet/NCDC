using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Parser.Sql.Segment.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:40:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    ///  Predicate bracket value for in.
    /// </summary>
    public sealed class PredicateBracketValue:IPredicateRightValue
    {
        private readonly PredicateLeftBracketValue _predicateLeftBracketValue;
    
        private readonly PredicateRightBracketValue _predicateRightBracketValue;

        public PredicateBracketValue(PredicateLeftBracketValue predicateLeftBracketValue, PredicateRightBracketValue predicateRightBracketValue)
        {
            _predicateLeftBracketValue = predicateLeftBracketValue;
            _predicateRightBracketValue = predicateRightBracketValue;
        }
    }
}
