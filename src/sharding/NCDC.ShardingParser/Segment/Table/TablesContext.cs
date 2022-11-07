using System.Collections.ObjectModel;
using NCDC.Basic.Metadatas;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.Segment.Select.SubQuery;
using NCDC.ShardingParser.Segment.Select.SubQuery.Engine;

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
    public class TablesContext
    {
        private readonly ICollection<SimpleTableSegment> _tables=new LinkedList<SimpleTableSegment>();
        private readonly ICollection<string> _tableNames=new HashSet<string>();
        private readonly ICollection<string> _schemaNames=new HashSet<string>();
        private readonly ICollection<string> _databaseNames=new HashSet<string>();

        private readonly IDictionary<string, ICollection<SubQueryTableContext>> _subQueryTables =
            new Dictionary<string, ICollection<SubQueryTableContext>>();


        public TablesContext(ICollection<SimpleTableSegment> tableSegments):this(tableSegments,new Dictionary<int, SelectCommandContext>())
        {
            
        }
        public TablesContext(IEnumerable<ITableSegment> tableSegments,IDictionary<int,SelectCommandContext> subQueryContexts)
        {
            var enumerable = tableSegments as ITableSegment[] ?? tableSegments.ToArray();
            if (enumerable.IsEmpty())
            {
                return;
            }

            foreach (var tableSegment in enumerable)
            {

                if (tableSegment is SimpleTableSegment simpleTableSegment)
                {
                    _tables.Add(simpleTableSegment);
                    _tableNames.Add(simpleTableSegment.TableName.IdentifierValue.Value);
                    if (simpleTableSegment.Owner is not null)
                    {
                        _schemaNames.Add(simpleTableSegment.Owner.IdentifierValue.Value);
                    }

                    var databaseName = FindDatabaseName(simpleTableSegment);
                    if (databaseName is not null)
                    {
                        _databaseNames.Add(databaseName);
                    }
                }
                if (tableSegment is SubQueryTableSegment subQueryTableSegment) {
                    
                    _subQueryTables.AddAll(CreateSubQueryTables(subQueryContexts, subQueryTableSegment));
                }
            }
        }
    
        private string? FindDatabaseName( SimpleTableSegment tableSegment)
        {
            return tableSegment.Owner?.IdentifierValue.Value;
        }
    
        private IDictionary<String, ICollection<SubQueryTableContext>> CreateSubQueryTables(IDictionary<int, SelectCommandContext> subQueryContexts,  SubQueryTableSegment subQueryTable) {
            var subQueryContext = subQueryContexts[subQueryTable.SubQuery.StartIndex];
            var subQueryTableContexts = SubQueryTableContextEngine.CreateSubQueryTableContexts(subQueryContext,subQueryTable.GetAlias());
            IDictionary<string, ICollection<SubQueryTableContext>> result = new Dictionary<string, ICollection<SubQueryTableContext>>();
            foreach (var subQueryTableContext in subQueryTableContexts)
            {
                if (subQueryTableContext.Alias is not null)
                {
                    if (!result.TryGetValue(subQueryTableContext.Alias, out var r))
                    {
                        r = new LinkedList<SubQueryTableContext>();
                        result.TryAdd(subQueryTableContext.Alias, r);
                    }

                    r.Add(subQueryTableContext);
                }
                
            }
            return result;
        }


        public string? GetDatabaseName() {
            if (_databaseNames.Count() > 1)
            {
                throw new InvalidOperationException("Can not support multiple different database.");
            }
            return _databaseNames.IsEmpty() ? null : _databaseNames.First();
        }
        /// <summary>
        /// ��ȡ���еı�����
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetTableNames()
        {
            foreach (var table in _tables)
            {
                yield return table.TableName.IdentifierValue.Value;
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
                return _tables.First().TableName.IdentifierValue.Value;
            }

            if (null != column.Owner)
            {
                return FindTableNameFromSql(column.Owner.IdentifierValue.Value);
            }

            return FindTableNameFromMetaData(column.IdentifierValue.Value, tableMetadata);
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
                var tableName = table.TableName.IdentifierValue.Value;
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
                    return table.TableName.IdentifierValue.Value;
                }
            }

            return null;
        }

        public ICollection<SimpleTableSegment> GetTables()
        {
            return _tables;
        }
    }
}