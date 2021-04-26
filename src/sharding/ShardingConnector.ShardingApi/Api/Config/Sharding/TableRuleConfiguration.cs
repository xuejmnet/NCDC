using System;
using System.Collections.Generic;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;

namespace ShardingConnector.ShardingApi.Api.Config.Sharding
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

        //public string ActualDataNodes { get; }
        public List<string> ActualDataNodes { get; }

        public IShardingStrategyConfiguration DatabaseShardingStrategyConfig { get; set; }

        public IShardingStrategyConfiguration TableShardingStrategyConfig { get; set; }

        public KeyGeneratorConfiguration KeyGeneratorConfig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logicTable">指定逻辑表名</param>
        public TableRuleConfiguration(string logicTable) : this(logicTable, null)
        {
        }
        /// <summary>
        ///// 
        ///// </summary>
        ///// <param name="logicTable">指定逻辑表名</param>
        ///// <param name="actualDataNodes"></param>
        //public TableRuleConfiguration(string logicTable, string actualDataNodes)
        //{
        //    this.LogicTable = logicTable ?? throw new ArgumentNullException("logicTable");
        //    this.ActualDataNodes = actualDataNodes;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logicTable">table</param>
        /// <param name="actualDataNodes">dataSource.table eg. ds0.table_0 ds0表示数据源table_0表示尾巴为0表</param>
        public TableRuleConfiguration(string logicTable, List<string> actualDataNodes)
        {
            this.LogicTable = logicTable ?? throw new ArgumentNullException("logicTable");
            this.ActualDataNodes = actualDataNodes;
        }
    }
}
