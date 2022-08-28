using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.Exceptions;

namespace ShardingConnector.CommandParserBinder.Segment.Table
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
    public class TablesContext
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
        public ICollection<string> GetTableNames()
        {
            ICollection<string> result = new LinkedList<string>();
            foreach (var table in _tables)
            {
                result.Add(table.GetTableName().GetIdentifier().GetValue());
            }

            return new HashSet<string>(result);
        }

        public string FindTableName(ColumnSegment column, SchemaMetaData schemaMetaData)
        {
            if (1 == _tables.Count)
            {
                return _tables.First().GetTableName().GetIdentifier().GetValue();
            }

            if (null != column.GetOwner())
            {
                return FindTableNameFromSQL(column.GetOwner().GetIdentifier().GetValue());
            }

            return FindTableNameFromMetaData(column.GetIdentifier().GetValue(), schemaMetaData);
        }

        /// <summary>
        /// 通过sql获取表名称
        /// </summary>
        /// <param name="tableNameOrAlias">表名或者表别名</param>
        /// <returns></returns>
        /// <exception cref="ShardingException"></exception>
        private string FindTableNameFromSQL(string tableNameOrAlias)
        {
            foreach (var table in _tables)
            {
                if (tableNameOrAlias.Equals(table.GetTableName().GetIdentifier().GetValue(), StringComparison.OrdinalIgnoreCase)
                    || tableNameOrAlias.Equals(table.GetAlias(), StringComparison.OrdinalIgnoreCase))
                {
                    return table.GetTableName().GetIdentifier().GetValue();
                }
            }

            //找不到表名
            throw new ShardingException("can not find owner from table.");
        }

        /// <summary>
        /// 通过元信息获取表名
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="schemaMetaData"></param>
        /// <returns></returns>
        private string FindTableNameFromMetaData(string columnName, SchemaMetaData schemaMetaData)
        {
            foreach (var table in _tables)
            {
                if (schemaMetaData.ContainsColumn(table.GetTableName().GetIdentifier().GetValue(), columnName))
                {
                    return table.GetTableName().GetIdentifier().GetValue();
                }
            }

            return null;
        }
    }
}