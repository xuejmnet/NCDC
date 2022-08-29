using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.ShardingApi.Api.Sharding.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:31:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IPreciseShardingAlgorithm<out T> : IShardingAlgorithm where T : IComparable
    {
        string DoSharding(ICollection<string> availableTargetNames, PreciseShardingValue shardingValue);
    }
}
