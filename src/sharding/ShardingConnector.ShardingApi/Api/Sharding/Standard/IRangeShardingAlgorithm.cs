using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingApi.Api.Sharding.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:25:36
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IRangeShardingAlgorithm<T>:IShardingAlgorithm where T:IComparable
    {
        ICollection<string> DoSharding(ICollection<string> availableTargetNames, RangeShardingValue<T> shardingValue);
    }
}
