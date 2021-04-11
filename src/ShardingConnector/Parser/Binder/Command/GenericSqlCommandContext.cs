using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Parser.Binder.Segment.Table;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Binder.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:44:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class GenericSqlCommandContext<T>:ISqlCommandContext<T> where T:ISqlCommand
    {
        private readonly T sqlCommand;

        private readonly TablesContext tablesContext;
        public GenericSqlCommandContext(T sqlCommand)
        {
            this.sqlCommand = sqlCommand;
            this.tablesContext = new TablesContext(new List<SimpleTableSegment>(0));
        }
        public T GetSqlCommand()
        {
            return sqlCommand;
        }

        public TablesContext GetTablesContext()
        {
            return tablesContext;
        }
    }
}
