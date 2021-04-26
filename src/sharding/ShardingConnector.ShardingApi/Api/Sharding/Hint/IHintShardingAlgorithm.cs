using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingApi.Api.Sharding.Hint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:37:04
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IHintShardingAlgorithm<T>:IShardingAlgorithm where T:IComparable
    {
        ICollection<string> DoSharding(ICollection<string> availableTargetNames, HintShardingValue<T> shardingValue);
    }
}
