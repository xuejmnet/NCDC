using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Standard;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;
using IComparable = System.IComparable;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:42:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class StandardShardingStrategy:IShardingStrategy
    {
        private readonly string shardingColumn;
    
        private readonly IPreciseShardingAlgorithm<IComparable> preciseShardingAlgorithm;
    
        private readonly IRangeShardingAlgorithm<IComparable> rangeShardingAlgorithm;
    
        public StandardShardingStrategy(StandardShardingStrategyConfiguration standardShardingStrategyConfig)
        {
            ShardingAssert.ShouldBeNotNull(standardShardingStrategyConfig.ShardingColumn, "Sharding column cannot be null.");
            ShardingAssert.ShouldBeNotNull(standardShardingStrategyConfig.PreciseShardingAlgorithm, "Sharding precise sharding algorithm cannot be null.");
            shardingColumn = standardShardingStrategyConfig.ShardingColumn;
            preciseShardingAlgorithm = standardShardingStrategyConfig.PreciseShardingAlgorithm;
            rangeShardingAlgorithm = standardShardingStrategyConfig.RangeShardingAlgorithm;
        }
        public ICollection<string> GetShardingColumns()
        {
            ICollection<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            result.Add(shardingColumn);
            return result;
        }

        public ICollection<string> DoSharding(ICollection<string> availableTargetNames, ICollection<IRouteValue> shardingValues,
            ConfigurationProperties properties)
        {
            IRouteValue shardingValue = shardingValues.First();
            ICollection<string> shardingResult = shardingValue is ListRouteValue listRouteValue
                ? DoSharding(availableTargetNames, listRouteValue) : DoSharding(availableTargetNames, (RangeRouteValue)shardingValue);
            ICollection<string> result = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            result.AddAll(shardingResult);
            return result;
        }

        private ICollection<string> DoSharding(ICollection<string> availableTargetNames, RangeRouteValue shardingValue)
        {
            if (null == rangeShardingAlgorithm)
            {
                throw new NotSupportedException("Cannot find range sharding strategy in sharding rule.");
            }
            return rangeShardingAlgorithm.DoSharding(availableTargetNames,
                new RangeShardingValue<IComparable>(shardingValue.GetTableName(), shardingValue.GetColumnName(), shardingValue.GetValueRange()));
        }

        private ICollection<string> DoSharding(ICollection<string> availableTargetNames, ListRouteValue shardingValue)
        {
            ICollection<string> result = new LinkedList<string>();
            foreach (var value in shardingValue.GetValues())
            {

                string target = preciseShardingAlgorithm.DoSharding(availableTargetNames, new PreciseShardingValue(shardingValue.GetTableName(), shardingValue.GetColumnName(), value));
                if (null != target)
                {
                    result.Add(target);
                }
            }
            return result;
        }
    }
}
