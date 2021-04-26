using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Complex;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route.Complex
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 12:07:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ComplexShardingStrategy:IShardingStrategy
    {
        public  ICollection<string> shardingColumns {get;}

        public IComplexKeysShardingAlgorithm<IComparable> shardingAlgorithm {get;}
    
        public ComplexShardingStrategy(ComplexShardingStrategyConfiguration complexShardingStrategyConfig)
        {
            ShardingAssert.CantBeNull(complexShardingStrategyConfig.ShardingColumns, "Sharding columns cannot be null.");
            ShardingAssert.CantBeNull(complexShardingStrategyConfig.ShardingAlgorithm, "Sharding algorithm cannot be null.");
            shardingColumns = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            shardingColumns.AddAll(complexShardingStrategyConfig.ShardingColumns.Split(new []{','},StringSplitOptions.RemoveEmptyEntries));
            shardingAlgorithm = complexShardingStrategyConfig.ShardingAlgorithm;
        }
        public ICollection<string> GetShardingColumns()
        {
            return shardingColumns;
        }

        public ICollection<string> DoSharding(ICollection<string> availableTargetNames, ICollection<IRouteValue> shardingValues,
            ConfigurationProperties properties)
        {
            IDictionary<string, ICollection<IComparable>> columnShardingValues =
                new Dictionary<string, ICollection<IComparable>>(shardingValues.Count);
            IDictionary<string,IComparable> columnRangeValues = new Dictionary<string, IComparable>(shardingValues.Count);
            string logicTableName = "";

            foreach (var shardingValue in shardingValues)
            {
                if (shardingValue is ListRouteValue<IComparable> listRouteValue) {
                    columnShardingValues.Add(shardingValue.GetColumnName(), listRouteValue.GetValues());
                } else if (shardingValue is RangeRouteValue<IComparable> rangeRouteValue) {
                    columnRangeValues.Add(shardingValue.GetColumnName(), rangeRouteValue.GetValueRange());
                }
                logicTableName = shardingValue.GetTableName();
            }
            var shardingResult = shardingAlgorithm.DoSharding(availableTargetNames, new ComplexKeysShardingValue<IComparable>(logicTableName, columnShardingValues, columnRangeValues));
            var result = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            result.AddAll(shardingResult);
            return result;
        }
}
}
