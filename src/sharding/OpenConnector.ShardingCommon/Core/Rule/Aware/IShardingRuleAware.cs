using System;

namespace OpenConnector.ShardingCommon.Core.Rule.Aware
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 26 April 2021 20:55:09
* @Email: 326308290@qq.com
*/
    public interface IShardingRuleAware
    {
        
        void SetShardingRule(ShardingRule shardingRule);
    }
}