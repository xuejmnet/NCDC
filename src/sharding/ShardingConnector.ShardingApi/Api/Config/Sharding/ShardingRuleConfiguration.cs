using System.Collections.Generic;
using ShardingConnector.Api.Config.Strategy;
using ShardingConnector.Common.Config;
using ShardingConnector.ShardingApi.Api.Config.MasterSlave;

namespace ShardingConnector.ShardingApi.Api.Config.Sharding
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:21:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingRuleConfiguration : IRuleConfiguration
    {
        public readonly ICollection<TableRuleConfiguration> TableRuleConfigs = new LinkedList<TableRuleConfiguration>();

        public readonly ICollection<string> BindingTableGroups = new LinkedList<string>();

        public readonly ICollection<string> BroadcastTables = new LinkedList<string>();

        public readonly ICollection<MasterSlaveRuleConfiguration> MasterSlaveRuleConfigs =
            new LinkedList<MasterSlaveRuleConfiguration>();

        public string DefaultDataSourceName { get; set; }

        public IShardingStrategyConfiguration DefaultDatabaseShardingStrategyConfig { get; set; }

        public IShardingStrategyConfiguration DefaultTableShardingStrategyConfig { get; set; }

        public KeyGeneratorConfiguration DefaultKeyGeneratorConfig { get; set; }


        // public EncryptRuleConfiguration EncryptRuleConfig { get; set; }
    }
}