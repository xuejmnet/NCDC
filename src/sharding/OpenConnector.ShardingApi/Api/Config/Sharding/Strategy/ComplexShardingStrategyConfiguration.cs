using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.Base;
using OpenConnector.ShardingApi.Api.Sharding.Complex;

namespace OpenConnector.ShardingApi.Api.Config.Sharding.Strategy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:31:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ComplexShardingStrategyConfiguration:IShardingStrategyConfiguration
    {
        public string ShardingColumns { get; }

        public IComplexKeysShardingAlgorithm<IComparable> ShardingAlgorithm { get; }

        public ComplexShardingStrategyConfiguration(string shardingColumns, IComplexKeysShardingAlgorithm<IComparable> shardingAlgorithm)
        {
            ShardingAssert.If(string.IsNullOrWhiteSpace(shardingColumns), "ShardingColumns is required.");
            ShardingAssert.ShouldBeNotNull(shardingAlgorithm, "ShardingAlgorithm is required.");
            this.ShardingColumns = shardingColumns;
            this.ShardingAlgorithm = shardingAlgorithm;
        }
    }
}
