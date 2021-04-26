using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.ShardingApi.Api.Sharding.Standard;

namespace ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:19:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class StandardShardingStrategyConfiguration:IShardingStrategyConfiguration
    {
        public  string ShardingColumn { get; }

        public  IPreciseShardingAlgorithm<IComparable> PreciseShardingAlgorithm { get; }

        public  IRangeShardingAlgorithm<IComparable> RangeShardingAlgorithm { get; }

        public StandardShardingStrategyConfiguration(string shardingColumn, IPreciseShardingAlgorithm<IComparable> preciseShardingAlgorithm):this(shardingColumn, preciseShardingAlgorithm, null)
        {
            
        }

        public StandardShardingStrategyConfiguration(string shardingColumn, IPreciseShardingAlgorithm<IComparable> preciseShardingAlgorithm, IRangeShardingAlgorithm<IComparable> rangeShardingAlgorithm)
        {
            ShardingAssert.If(string.IsNullOrWhiteSpace(shardingColumn), "ShardingColumns is required.");
            ShardingAssert.CantBeNull(preciseShardingAlgorithm, "PreciseShardingAlgorithm is required.");
            this.ShardingColumn = shardingColumn;
            this.PreciseShardingAlgorithm = preciseShardingAlgorithm;
            this.RangeShardingAlgorithm = rangeShardingAlgorithm;
        }
    }
}
