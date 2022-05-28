using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Hint;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route.Hint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 12:14:54
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class HintShardingStrategy:IShardingStrategy
    {
        public ICollection<string> ShardingColumns { get; }

        public IHintShardingAlgorithm<IComparable> ShardingAlgorithm { get; }
        public HintShardingStrategy(HintShardingStrategyConfiguration hintShardingStrategyConfig)
        {
            ShardingAssert.ShouldBeNotNull(hintShardingStrategyConfig.ShardingAlgorithm, "Sharding algorithm cannot be null.");
            ShardingColumns = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            ShardingAlgorithm = hintShardingStrategyConfig.ShardingAlgorithm;
        }
        public ICollection<string> GetShardingColumns()
        {
            return ShardingColumns;
        }

        public ICollection<string> DoSharding(ICollection<string> availableTargetNames, ICollection<IRouteValue> shardingValues,
            ConfigurationProperties properties)
        {
            var shardingValue = (ListRouteValue)shardingValues.First();
            var shardingResult = ShardingAlgorithm.DoSharding(availableTargetNames,
                new HintShardingValue<IComparable>(shardingValue.GetTableName(), shardingValue.GetColumnName(), shardingValue.GetValues()));
            var result = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            result.AddAll(shardingResult);
            return result;
        }
    }
}
