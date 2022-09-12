namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:58:09
* @Email: 326308290@qq.com
*/
    public sealed class DistinctCountAggregationUnit : IAggregationUnit
    {
        private ISet<IComparable> values = new HashSet<IComparable>();

        public void Merge(List<IComparable> values)
        {
            if (null == values || null == values[0])
            {
                return;
            }

            this.values.Add(values[0]);
        }

        public IComparable GetResult()
        {
            return values.Count;
        }
    }
}