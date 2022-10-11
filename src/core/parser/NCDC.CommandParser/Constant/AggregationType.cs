using System;
using NCDC.CommandParser.Exceptions;

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
        private static readonly IDictionary<string, AggregationTypeEnum> _caches =
            new Dictionary<string, AggregationTypeEnum>(StringComparer.OrdinalIgnoreCase);
        static AggregationType()
        {
            _caches.Add(nameof(AggregationTypeEnum.MAX),AggregationTypeEnum.MAX);
            _caches.Add(nameof(AggregationTypeEnum.MIN),AggregationTypeEnum.MIN);
            _caches.Add(nameof(AggregationTypeEnum.SUM),AggregationTypeEnum.SUM);
            _caches.Add(nameof(AggregationTypeEnum.COUNT),AggregationTypeEnum.COUNT);
            _caches.Add(nameof(AggregationTypeEnum.AVG),AggregationTypeEnum.AVG);
            _caches.Add(nameof(AggregationTypeEnum.BIT_XOR),AggregationTypeEnum.BIT_XOR);
        }
        public static bool IsAggregationType(string aggregationType)
        {
            return _caches.ContainsKey(aggregationType);
        }

        public static AggregationTypeEnum ValueOf(string aggregationType)
        {
            if (IsAggregationType(aggregationType))
            {
                return _caches[aggregationType];
            }

            throw new SqlParsingBaseException($"unknown aggregation type:{aggregationType}");
        }
    }

    public enum AggregationTypeEnum
    {
        MAX, MIN, SUM, COUNT, AVG,BIT_XOR
    }
}