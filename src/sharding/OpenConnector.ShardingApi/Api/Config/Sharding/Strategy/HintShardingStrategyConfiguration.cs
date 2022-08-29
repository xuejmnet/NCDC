using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.Base;
using OpenConnector.ShardingApi.Api.Sharding.Hint;

namespace OpenConnector.ShardingApi.Api.Config.Sharding.Strategy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:34:33
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class HintShardingStrategyConfiguration:IShardingStrategyConfiguration
    {
        public IHintShardingAlgorithm<IComparable> ShardingAlgorithm { get; }
    
        public HintShardingStrategyConfiguration(IHintShardingAlgorithm<IComparable> shardingAlgorithm)
        {
            ShardingAssert.ShouldBeNotNull(shardingAlgorithm, "ShardingAlgorithm is required.");
            this.ShardingAlgorithm = shardingAlgorithm;
        }
    }
}
