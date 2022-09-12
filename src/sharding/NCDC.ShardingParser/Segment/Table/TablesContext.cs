using NCDC.Basic.Parsers;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.Basic.TableMetadataManagers;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;

namespace NCDC.ShardingParser.Segment.Table
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:26:31
* @Email: 326308290@qq.com
*/
    /// <summary>
    /// 当前表上下文
    /// </summary>
    public class TablesContext:ITablesContext
    {
        private readonly ICollection<SimpleTableSegment> _tables;

        public TablesContext(SimpleTableSegment tableSegment) : this(new List<SimpleTableSegment>(1) {tableSegment})
        {
        }

        public TablesContext(ICollection<SimpleTableSegment> tables)
        {
            _tables = tables;
        }

        /// <summary>
        /// ��ȡ���еı�����
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetTableNames()
        {
            foreach (var table in _tables)
            {
                yield return table.GetTableName().GetIdentifier().GetValue();
            }
        }

        public int GetTableNameCount()
        {
            return _tables.Count;
        }

        public string? FindTableName(ColumnSegment column, TableMetadata tableMetadata)
        {
            if (1 == _tables.Count)
            {
                return _tables.First().GetTableName().GetIdentifier().GetValue();
            }

            if (null != column.GetOwner())
            {
                return FindTableNameFromSql(column.GetOwner()!.GetIdentifier().GetValue());
            }

            return FindTableNameFromMetaData(column.GetIdentifier().GetValue(), tableMetadata);
        }

        /// <summary>
        /// 通过sql获取表名称
        /// </summary>
        /// <param name="tableNameOrAlias">表名或者表别名</param>
        /// <returns></returns>
        /// <exception cref="ShardingException"></exception>
        private string FindTableNameFromSql(string tableNameOrAlias)
        {
            foreach (var table in _tables)
            {
                var tableName = table.GetTableName().GetIdentifier().GetValue();
                if (tableNameOrAlias.EqualsIgnoreCase(tableName)
                    || tableNameOrAlias.EqualsIgnoreCase(table.GetAlias()))
                {
                    return tableName;
                }
            }

            //找不到表名
            throw new ShardingException("can not find owner from table.");
        }

        /// <summary>
        /// 通过元信息获取表名
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="TableMetadata"></param>
        /// <returns></returns>
        private string? FindTableNameFromMetaData(string columnName, TableMetadata tableMetadata)
        {
            foreach (var table in _tables)
            {
                if (tableMetadata.ContainsColumn(columnName))
                {
                    return table.GetTableName().GetIdentifier().GetValue();
                }
            }

            return null;
        }
    }
}