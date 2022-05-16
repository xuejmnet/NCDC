using System.Collections.Generic;
using ShardingConnector.Common.Config;
using ShardingConnector.ShardingApi.Api.Config.MasterSlave;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;

namespace ShardingConnector.ShardingApi.Api.Config.Sharding
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:21:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 分片规则的配置
    /// </summary>
    public sealed class ShardingRuleConfiguration : IRuleConfiguration
    {
        /// <summary>
        /// 哪些表需要分片
        /// </summary>
        public readonly List<TableRuleConfiguration> TableRuleConfigs = new List<TableRuleConfiguration>(31);

        /// <summary>
        /// 哪些是绑定表,为了保证主从表分表后后缀涉及一样防止笛卡尔积
        /// </summary>
        public readonly List<string> BindingTableGroups = new List<string>(31);

        /// <summary>
        /// 哪些是广播表,用于设置对应表存在于所有的数据源当中,crud会涉及所有的数据源表的操作,如果不进行处理会导致无法跨库join
        /// </summary>
        public readonly List<string> BroadcastTables = new List<string>(31);

        /// <summary>
        /// 读写分离的配置
        /// </summary>
        public readonly List<MasterSlaveRuleConfiguration> MasterSlaveRuleConfigs =
            new List<MasterSlaveRuleConfiguration>(31);

        /// <summary>
        /// 默认的数据源
        /// </summary>
        public string DefaultDataSourceName { get; set; }

        /// <summary>
        /// 默认的数据库分片策略
        /// </summary>
        public IShardingStrategyConfiguration DefaultDatabaseShardingStrategyConfig { get; set; }

        /// <summary>
        /// 默认的表分片策略
        /// </summary>
        public IShardingStrategyConfiguration DefaultTableShardingStrategyConfig { get; set; }

        /// <summary>
        /// 默认的表创建配置
        /// </summary>
        public KeyGeneratorConfiguration DefaultKeyGeneratorConfig { get; set; }


        // public EncryptRuleConfiguration EncryptRuleConfig { get; set; }
    }
}