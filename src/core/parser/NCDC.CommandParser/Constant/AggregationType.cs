using System;

namespace NCDC.CommandParser.Constant
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 16:27:33
* @Email: 326308290@qq.com
*/
    public class AggregationType
    {
        public static bool IsAggregationType(string aggregationType)
        {
            return nameof(AggregationTypeEnum.MAX).Equals(aggregationType, StringComparison.OrdinalIgnoreCase)
                   || nameof(AggregationTypeEnum.MIN).Equals(aggregationType, StringComparison.OrdinalIgnoreCase)
                   || nameof(AggregationTypeEnum.SUM).Equals(aggregationType, StringComparison.OrdinalIgnoreCase)
                   || nameof(AggregationTypeEnum.COUNT).Equals(aggregationType, StringComparison.OrdinalIgnoreCase)
                   || nameof(AggregationTypeEnum.AVG).Equals(aggregationType, StringComparison.OrdinalIgnoreCase);
        }

        public static AggregationTypeEnum ValueOf(string aggregationType)
        {
            return (AggregationTypeEnum)Enum.Parse(typeof(AggregationTypeEnum),aggregationType);
        }
    }

    public enum AggregationTypeEnum
    {
        MAX, MIN, SUM, COUNT, AVG
    }
}