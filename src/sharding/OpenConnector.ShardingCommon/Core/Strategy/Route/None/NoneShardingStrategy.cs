using System.Collections.Generic;
using NCDC.Common.Config.Properties;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace OpenConnector.ShardingCommon.Core.Strategy.Route.None
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:05:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class NoneShardingStrategy:IShardingStrategy
    {
        public ICollection<string> GetShardingColumns()
        {
            return new List<string>();
        }

        public ICollection<string> DoSharding(ICollection<string> availableTargetNames, ICollection<IRouteValue> shardingValues,
            ConfigurationProperties properties)
        {
            return availableTargetNames;
        }
    }
}