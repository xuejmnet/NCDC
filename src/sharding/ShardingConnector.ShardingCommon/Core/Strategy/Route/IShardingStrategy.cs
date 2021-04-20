using System.Collections.Generic;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace ShardingConnector.ShardingCommon.Core.Strategy.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IShardingStrategy
    {
        
        /**
     * Get sharding columns.
     * 
     * @return sharding columns
     */
        ICollection<string> GetShardingColumns();
    
        /**
     * Sharding.
     *
     * @param availableTargetNames available data sources or tables's names
     * @param shardingValues sharding values
     * @param properties ShardingSphere properties
     * @return sharding results for data sources or tables's names
     */
        ICollection<string> DoSharding(ICollection<string> availableTargetNames, ICollection<IRouteValue> shardingValues, ConfigurationProperties properties);

    }
}