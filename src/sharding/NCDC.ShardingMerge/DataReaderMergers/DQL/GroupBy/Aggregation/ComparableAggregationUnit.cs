namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:52:04
* @Email: 326308290@qq.com
*/
    public  sealed class ComparableAggregationUnit:IAggregationUnit
    {
        private readonly bool _asc;
    
        private IComparable _result;

        public ComparableAggregationUnit(bool asc)
        {
            this._asc = asc;
        }

        public void Merge(List<IComparable> values)
        {
            if (null == values || null == values[0]) {
                return;
            }
            if (null == _result) {
                _result = values[0];
                return;
            }
            int comparedValue = ((IComparable) values[0]).CompareTo(_result);
            if (_asc && comparedValue < 0 || !_asc && comparedValue > 0) {
                _result = values[0];
            }
        }

        public IComparable GetResult()
        {
            return _result;
        }
    }
}