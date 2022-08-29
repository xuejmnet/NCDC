using System;
using System.Collections.Generic;

namespace OpenConnector.ShardingMerge.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:45:41
* @Email: 326308290@qq.com
*/
    public sealed class AverageAggregationUnit : IAggregationUnit
    {
        private decimal _count;

        private decimal _sum;

        public void Merge(List<IComparable> values)
        {
            if (null == values || null == values[0] || null == values[1])
            {
                return;
            }

            _count += Convert.ToDecimal(values[0]);
            _sum += Convert.ToDecimal(values[1]);
        }

        public IComparable GetResult()
        {
            if (decimal.Zero == _count)
            {
                return _count;
            }

            // TODO use metadata to fetch float number precise for database field
            return Math.Round(_sum / _count, 4, MidpointRounding.AwayFromZero);
        }
    }
}