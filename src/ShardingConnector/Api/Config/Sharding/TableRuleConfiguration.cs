using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Api.Config.Strategy;

namespace ShardingConnector.Api.Config.Sharding
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:22:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableRuleConfiguration
    {
        public string LogicTable { get; }

        public string ActualDataNodes { get; }

        public IShardingStrategyConfiguration DatabaseShardingStrategyConfig { get; set; }

        public IShardingStrategyConfiguration TableShardingStrategyConfig { get; set; }

        public KeyGeneratorConfiguration KeyGeneratorConfig { get; set; }

        public TableRuleConfiguration(string logicTable) : this(logicTable, null)
        {
        }

        public TableRuleConfiguration(string logicTable, string actualDataNodes)
        {
            this.LogicTable = logicTable ?? throw new ArgumentNullException("logicTable");
            this.ActualDataNodes = actualDataNodes;
        }
    }
}
