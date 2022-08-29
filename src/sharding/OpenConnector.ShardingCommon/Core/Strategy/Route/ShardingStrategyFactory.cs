using OpenConnector.ShardingApi.Api.Config.Sharding.Strategy;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Complex;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Hint;
using OpenConnector.ShardingCommon.Core.Strategy.Route.None;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Standard;

namespace OpenConnector.ShardingCommon.Core.Strategy.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:03:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ShardingStrategyFactory
    {
        private ShardingStrategyFactory()
        {
            
        }
        
        public static IShardingStrategy NewInstance(IShardingStrategyConfiguration shardingStrategyConfig) {
            if (shardingStrategyConfig is StandardShardingStrategyConfiguration shardingStrategyConfiguration) {
                return new StandardShardingStrategy(shardingStrategyConfiguration);
            }
            if (shardingStrategyConfig is ComplexShardingStrategyConfiguration complexShardingStrategyConfiguration) {
                return new ComplexShardingStrategy(complexShardingStrategyConfiguration);
            }
            if (shardingStrategyConfig is HintShardingStrategyConfiguration hintShardingStrategyConfiguration) {
                return new HintShardingStrategy(hintShardingStrategyConfiguration);
            }
            return new NoneShardingStrategy();
        }
    }
}