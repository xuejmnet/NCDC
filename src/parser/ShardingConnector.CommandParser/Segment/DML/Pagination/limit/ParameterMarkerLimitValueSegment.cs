namespace ShardingConnector.CommandParser.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:54:32
* @Email: 326308290@qq.com
*/
    public sealed class ParameterMarkerLimitValueSegment:LimitValueSegment,IParameterMarkerPaginationValueSegment
    {
        private readonly int _parameterIndex;
        public ParameterMarkerLimitValueSegment(int startIndex, int stopIndex,int parameterIndex) : base(startIndex, stopIndex)
        {
            _parameterIndex = parameterIndex;
        }

        public int GetParameterIndex()
        {
            return _parameterIndex;
        }
    }
}