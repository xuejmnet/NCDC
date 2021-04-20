using ShardingConnector.Api.Config.Strategy;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.None;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route
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
            // if (shardingStrategyConfig instanceof StandardShardingStrategyConfiguration) {
            //     return new StandardShardingStrategy((StandardShardingStrategyConfiguration) shardingStrategyConfig);
            // }
            // if (shardingStrategyConfig instanceof InlineShardingStrategyConfiguration) {
            //     return new InlineShardingStrategy((InlineShardingStrategyConfiguration) shardingStrategyConfig);
            // }
            // if (shardingStrategyConfig instanceof ComplexShardingStrategyConfiguration) {
            //     return new ComplexShardingStrategy((ComplexShardingStrategyConfiguration) shardingStrategyConfig);
            // }
            // if (shardingStrategyConfig instanceof HintShardingStrategyConfiguration) {
            //     return new HintShardingStrategy((HintShardingStrategyConfiguration) shardingStrategyConfig);
            // }
            return new NoneShardingStrategy();
        }
    }
}