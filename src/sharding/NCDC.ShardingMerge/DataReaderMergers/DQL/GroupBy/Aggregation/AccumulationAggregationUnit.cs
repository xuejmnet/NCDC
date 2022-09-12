namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:43:09
* @Email: 326308290@qq.com
*/
    public sealed class AccumulationAggregationUnit:IAggregationUnit
    {
        private decimal _result;


        public void Merge(List<IComparable> values)
        {
            if (null == values || null == values[0]) {
                return;
            }

            _result += Convert.ToDecimal(values[0]);
        }

        public IComparable GetResult()
        {
            return _result;
        }
    }
}