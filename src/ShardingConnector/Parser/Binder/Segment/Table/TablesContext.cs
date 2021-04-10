using System.Collections.Generic;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Binder.Segment.Table
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:26:31
* @Email: 326308290@qq.com
*/
/// <summary>
/// 表的上下文
/// </summary>
    public class TablesContext
    {
        private readonly ICollection<SimpleTableSegment> _tables;

        public TablesContext(SimpleTableSegment tableSegment):this(new List<SimpleTableSegment>(1) { tableSegment})
        {
            
        }

        public TablesContext(ICollection<SimpleTableSegment> tables)
        {
            _tables = tables;
        }
        /// <summary>
        /// 获取所有的表名称
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

        public string FindTableName(colum)
    }
}