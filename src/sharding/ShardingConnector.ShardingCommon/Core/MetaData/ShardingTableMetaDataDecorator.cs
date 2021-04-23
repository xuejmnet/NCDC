using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Common.MetaData.Decorator;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.MetaData.Column;
using ShardingConnector.ParserBinder.MetaData.Index;
using ShardingConnector.ParserBinder.MetaData.Table;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingCommon.Core.MetaData
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/23 17:00:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingTableMetaDataDecorator: ITableMetaDataDecorator<ShardingRule>
    {
        public TableMetaData Decorate(TableMetaData tableMetaData, string tableName, ShardingRule rule)
        {
            var tableRule=rule.FindTableRule(tableName);
            if (tableRule != null)
            {
                return new TableMetaData(GetColumnMetaDataList(tableMetaData, tableRule),
                    GetIndexMetaDataList(tableMetaData, tableRule));
            }
            return tableMetaData;
        }

        private ICollection<ColumnMetaData> GetColumnMetaDataList(TableMetaData tableMetaData, TableRule tableRule)
        {
            var generateKeyColumn = tableRule.GetGenerateKeyColumn();
            if (null==generateKeyColumn)
            {
                return tableMetaData.GetColumns().Select(o=>o.Value).ToList();
            }
            ICollection<ColumnMetaData> result = new LinkedList<ColumnMetaData>();
            foreach (var column in tableMetaData.GetColumns())
            {
                if (column.Key.EqualsIgnoreCase(generateKeyColumn))
                {
                    result.Add(new ColumnMetaData(
                        column.Value.Name, column.Value.ColumnOrdinal, column.Value.DataTypeName, column.Value.PrimaryKey, true, column.Value.CaseSensitive));
                }
                else
                {
                    result.Add(column.Value);
                }
            }
            return result;
        }

        private ICollection<IndexMetaData> GetIndexMetaDataList(TableMetaData tableMetaData, TableRule tableRule)
        {
            ICollection<IndexMetaData> result = new HashSet<IndexMetaData>();
            foreach (var indexMeta in tableMetaData.GetIndexes())
            {
                foreach (var actualDataNode in tableRule.ActualDataNodes)
                {
                    var logicIndex = GetLogicIndex(indexMeta.Key,actualDataNode.GetTableName());
                    if (logicIndex != null)
                    {
                        result.Add(new IndexMetaData(logicIndex));
                    }
                }
            }
            return result;
        }

        private string GetLogicIndex(string actualIndexName, string actualTableName)
        {
            var indexNameSuffix = "_" + actualTableName;
            return actualIndexName.EndsWith(indexNameSuffix) ? actualIndexName.Replace(indexNameSuffix,string.Empty): null;
        }
    }
}
