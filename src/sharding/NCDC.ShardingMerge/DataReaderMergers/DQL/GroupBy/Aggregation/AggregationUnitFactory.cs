using NCDC.CommandParser.Common.Constant;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:01:43
* @Email: 326308290@qq.com
*/
    public class AggregationUnitFactory
    {
        private AggregationUnitFactory()
        {
            
        }
        
        /**
     * Create aggregation unit instance.
     * 
     * @param type aggregation function type
     * @param isDistinct is distinct
     * @return aggregation unit instance
     */
        public static IAggregationUnit Create(AggregationTypeEnum type,  bool isDistinct) {
            switch (type) {
                case AggregationTypeEnum.MAX:
                    return new ComparableAggregationUnit(false);
                case AggregationTypeEnum.MIN:
                    return new ComparableAggregationUnit(true);
                case AggregationTypeEnum.SUM:
                    return GetSumAggregationUnit(isDistinct);
                case AggregationTypeEnum.COUNT:
                    return GetCountAggregationUnit(isDistinct);
                case AggregationTypeEnum.AVG:
                    return GetAvgAggregationUnit(isDistinct);
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        private static IAggregationUnit GetSumAggregationUnit(bool isDistinct)
        {
            if (isDistinct)
            {
                return new DistinctSumAggregationUnit();
            }
            else
            {
                return new AccumulationAggregationUnit();
            }
        }
        private static IAggregationUnit GetCountAggregationUnit(bool isDistinct)
        {
            if (isDistinct)
            {
                return new DistinctCountAggregationUnit();
            }
            else
            {
                return new AccumulationAggregationUnit();
            }
        }
        private static IAggregationUnit GetAvgAggregationUnit(bool isDistinct)
        {
            if (isDistinct)
            {
                return new DistinctAverageAggregationUnit();
            }
            else
            {
                return new AverageAggregationUnit();
            }
        }
    }
}