using System;
using System.Collections.Generic;

namespace ShardingConnector.ShardingMerge.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:54:04
* @Email: 326308290@qq.com
*/
    public sealed class DistinctAverageAggregationUnit:IAggregationUnit
    {
        private decimal _count;
    
        private decimal _sum;
    
        private readonly ISet<IComparable> _countValues = new HashSet<IComparable>();

        private readonly ISet<IComparable> _sumValues = new HashSet<IComparable>();


        public void Merge(List<IComparable> values)
        {
            if (null == values || null == values[0] || null == values[1]) {
                return;
            }
            if (this._countValues.Add(values[0]) && this._sumValues.Add(values[0])) {
               
                _count +=Convert.ToDecimal(values[0]);
                _sum +=Convert.ToDecimal(values[1]);
            }
        }

        public IComparable GetResult()
        {
            if (decimal.Zero == _count) {
                return _count;
            }
            // TODO use metadata to fetch float number precise for database field
            
            return Math.Round(_sum / _count, 4, MidpointRounding.AwayFromZero);
        }
    }
}