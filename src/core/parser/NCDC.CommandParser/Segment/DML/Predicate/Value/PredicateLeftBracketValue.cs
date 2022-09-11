namespace NCDC.CommandParser.Segment.DML.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:41:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    ///  Predicate left bracket value for IN operator.
    /// </summary>
    public sealed class PredicateLeftBracketValue:IPredicateRightValue
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public PredicateLeftBracketValue(int startIndex, int stopIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
        }
    }
}
