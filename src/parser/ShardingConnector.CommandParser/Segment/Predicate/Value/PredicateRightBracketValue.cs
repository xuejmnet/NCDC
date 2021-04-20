namespace ShardingConnector.CommandParser.Segment.Predicate.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:42:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PredicateRightBracketValue:IPredicateRightValue
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public PredicateRightBracketValue(int startIndex, int stopIndex)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
        }
    }
}
