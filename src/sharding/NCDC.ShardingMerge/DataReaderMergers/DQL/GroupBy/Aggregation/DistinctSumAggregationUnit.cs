namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:59:20
* @Email: 326308290@qq.com
*/
    public sealed class DistinctSumAggregationUnit:IAggregationUnit
    {
        private decimal _result;
    
        private ISet<IComparable> values = new HashSet<IComparable>();

        public void Merge(List<IComparable> values)
        { if (null == values || null == values[0]) {
                return;
            }
            if (this.values.Add(values[0])) {
                _result +=Convert.ToDecimal(values[0]);
            }
        }

        public IComparable GetResult()
        {
            return _result;
        }
    }
}