using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingApi.Spi.KeyGen;
using ShardingConnector.ShardingCommon.Core.Strategy.Route;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.None;
using ShardingConnector.ShardingCommon.Spi.Algorithm.KeyGen;

namespace ShardingConnector.ShardingCommon.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 10:39:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class TableRule
    {
        public string LogicTable { get; }

        public List<DataNode> ActualDataNodes { get; }

        private readonly ISet<string> actualTables;

        private readonly IDictionary<DataNode, int> dataNodeIndexMap;

        public IShardingStrategy DatabaseShardingStrategy { get; }

        public IShardingStrategy TableShardingStrategy { get; }

        private readonly string generateKeyColumn;

        public IShardingKeyGenerator ShardingKeyGenerator { get; }

        public readonly ICollection<string> ActualDatasourceNames = new HashSet<string>();

        public readonly IDictionary<string, ICollection<string>> DatasourceToTablesMap =
            new Dictionary<string, ICollection<string>>();

        public TableRule(string defaultDataSourceName, string logicTableName)
        {
            LogicTable = logicTableName.ToLower();
            ActualDataNodes = new List<DataNode>() {new DataNode(defaultDataSourceName, logicTableName)};
            actualTables = GetActualTables();
            CacheActualDataSourcesAndTables();
            dataNodeIndexMap = new Dictionary<DataNode, int>();
            DatabaseShardingStrategy = null;
            TableShardingStrategy = null;
            generateKeyColumn = null;
            ShardingKeyGenerator = null;
        }

        public TableRule(ICollection<string> dataSourceNames, string logicTableName)
        {
            LogicTable = logicTableName.ToLower();
            dataNodeIndexMap = new Dictionary<DataNode, int>(dataSourceNames.Count);
            ActualDataNodes = GenerateDataNodes(logicTableName, dataSourceNames);
            actualTables = GetActualTables();
            DatabaseShardingStrategy = null;
            TableShardingStrategy = null;
            generateKeyColumn = null;
            ShardingKeyGenerator = null;
        }

        public TableRule(TableRuleConfiguration tableRuleConfig, ShardingDataSourceNames shardingDataSourceNames,
            string defaultGenerateKeyColumn)
        {
            LogicTable = tableRuleConfig.LogicTable.ToLower();
            // List<string> dataNodes = new InlineExpressionParser(tableRuleConfig.GetActualDataNodes()).splitAndEvaluate();
            // dataNodeIndexMap = new HashMap<>(dataNodes.size(), 1);
            dataNodeIndexMap = new Dictionary<DataNode, int>();
            // actualDataNodes = IsEmptyDataNodes(dataNodes)
            //     ? GenerateDataNodes(tableRuleConfig.LogicTable, shardingDataSourceNames.DataSourceNames) : GenerateDataNodes(dataNodes, shardingDataSourceNames.getDataSourceNames());
            ActualDataNodes = GenerateDataNodes(tableRuleConfig.LogicTable, shardingDataSourceNames.DataSourceNames);
            actualTables = GetActualTables();
            DatabaseShardingStrategy = null == tableRuleConfig.DatabaseShardingStrategyConfig
                ? null
                : ShardingStrategyFactory.NewInstance(tableRuleConfig.DatabaseShardingStrategyConfig);
            TableShardingStrategy = null == tableRuleConfig.TableShardingStrategyConfig
                ? null
                : ShardingStrategyFactory.NewInstance(tableRuleConfig.DatabaseShardingStrategyConfig);
            KeyGeneratorConfiguration keyGeneratorConfiguration = tableRuleConfig.KeyGeneratorConfig;
            generateKeyColumn =
                null != keyGeneratorConfiguration && !string.IsNullOrWhiteSpace(keyGeneratorConfiguration.Column)
                    ? keyGeneratorConfiguration.Column
                    : defaultGenerateKeyColumn;
            ShardingKeyGenerator = ContainsKeyGeneratorConfiguration(tableRuleConfig)
                ? new ShardingKeyGeneratorServiceLoader().NewService(tableRuleConfig.KeyGeneratorConfig.Type,
                    tableRuleConfig.KeyGeneratorConfig.Properties)
                : null;
            // CheckRule(dataNodes);
        }

        private void CacheActualDataSourcesAndTables()
        {
            foreach (var actualDataNode in ActualDataNodes)
            {
                ActualDatasourceNames.Add(actualDataNode.GetDataSourceName());
                AddActualTable(actualDataNode.GetDataSourceName(), actualDataNode.GetTableName());
            }
        }

        private ISet<string> GetActualTables()
        {
            return ActualDataNodes.Select(o => o.GetTableName()).ToHashSet();
        }

        private void AddActualTable(string datasourceName, string tableName)
        {
            ICollection<string> datasourceToTables = null;
            if (DatasourceToTablesMap.ContainsKey(datasourceName))
            {
                datasourceToTables = DatasourceToTablesMap[datasourceName];
            }

            if (datasourceToTables == null)
            {
                datasourceToTables = new LinkedList<string>();
                DatasourceToTablesMap.Add(datasourceName, datasourceToTables);
            }

            datasourceToTables.Add(tableName);
        }

        private bool ContainsKeyGeneratorConfiguration(TableRuleConfiguration tableRuleConfiguration)
        {
            return null != tableRuleConfiguration.KeyGeneratorConfig &&
                   !string.IsNullOrWhiteSpace(tableRuleConfiguration.KeyGeneratorConfig.Type);
        }

        private bool IsEmptyDataNodes(List<string> dataNodes)
        {
            return null == dataNodes || dataNodes.IsEmpty();
        }

        private List<DataNode> GenerateDataNodes(string logicTable, ICollection<string> dataSourceNames)
        {
            ICollection<DataNode> result = new LinkedList<DataNode>();
            int index = 0;
            foreach (var dataSourceName in dataSourceNames)
            {
                DataNode dataNode = new DataNode(dataSourceName, logicTable);
                result.Add(dataNode);
                dataNodeIndexMap.Add(dataNode, index);
                ActualDatasourceNames.Add(dataSourceName);
                AddActualTable(dataNode.GetDataSourceName(), dataNode.GetTableName());
                index++;
            }

            return result.ToList();
        }

        private List<DataNode> GenerateDataNodes(List<string> actualDataNodes, ICollection<string> dataSourceNames)
        {
            ICollection<DataNode> result = new LinkedList<DataNode>();
            int index = 0;
            foreach (var actualDataNode in actualDataNodes)
            {
                DataNode dataNode = new DataNode(actualDataNode);
                if (!dataSourceNames.Contains(dataNode.GetDataSourceName()))
                {
                    throw new ShardingException(
                        "Cannot find data source in sharding rule, invalid actual data node is: '{actualDataNode}'");
                }

                result.Add(dataNode);
                dataNodeIndexMap.Add(dataNode, index);
                ActualDatasourceNames.Add(dataNode.GetDataSourceName());
                AddActualTable(dataNode.GetDataSourceName(), dataNode.GetTableName());
                index++;
            }

            return result.ToList();
        }

        /**
     * Get data node groups.
     *
     * @return data node groups, key is data source name, value is data nodes belong to this data source
     */
        public IDictionary<string, List<DataNode>> GetDataNodeGroups()
        {
            IDictionary<string, List<DataNode>> result = new Dictionary<string, List<DataNode>>(ActualDataNodes.Count);
            foreach (var actualDataNode in ActualDataNodes)
            {
                string dataSourceName = actualDataNode.GetDataSourceName();
                if (!result.ContainsKey(dataSourceName))
                {
                    result.Add(dataSourceName, new List<DataNode>());
                }

                result[dataSourceName].Add(actualDataNode);
            }

            return result;
        }

        /**
     * Get actual data source names.
     *
     * @return actual data source names
     */
        public ICollection<string> GetActualDatasourceNames()
        {
            return ActualDatasourceNames;
        }

        /**
     * Get actual table names via target data source name.
     *
     * @param targetDataSource target data source name
     * @return names of actual tables
     */
        public ICollection<string> GetActualTableNames(string targetDataSource)
        {
            if (DatasourceToTablesMap.TryGetValue(targetDataSource, out var result))
                return result;
            return new List<string>(0);
        }

        public int FindActualTableIndex(string dataSourceName, string actualTableName)
        {
            if (dataNodeIndexMap.TryGetValue(new DataNode(dataSourceName, actualTableName), out var value))
                return value;
            return -1;
        }

        public bool IsExisted(string actualTableName)
        {
            return actualTables.Contains(actualTableName);
        }

        private void CheckRule(List<string> dataNodes)
        {
            if (IsEmptyDataNodes(dataNodes) && null != TableShardingStrategy &&
                !(TableShardingStrategy is NoneShardingStrategy))
            {
                throw new ShardingException(
                    $"ActualDataNodes must be configured if want to shard tables for logicTable [{LogicTable}]");
            }
        }

        /**
     * Get generate key column.
     * 
     * @return generate key column
     */
        public string GetGenerateKeyColumn()
        {
            return generateKeyColumn;
        }
    }
}