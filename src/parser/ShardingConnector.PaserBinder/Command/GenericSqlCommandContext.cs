using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.ParserBinder.Segment.Table;

namespace ShardingConnector.ParserBinder.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:44:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class GenericSqlCommandContext<T>: ISqlCommandContext<T> where T:ISqlCommand
    {
        private readonly T _sqlCommand;

        private readonly TablesContext _tablesContext;
        public GenericSqlCommandContext(T sqlCommand)
        {
            this._sqlCommand = sqlCommand;
            this._tablesContext = new TablesContext(new List<SimpleTableSegment>(0));
        }
        public T GetSqlCommand()
        {
            return _sqlCommand;
        }

        public override string ToString()
        {
            return $"{nameof(_sqlCommand)}: {_sqlCommand}, {nameof(_tablesContext)}: {_tablesContext}";
        }

        public virtual TablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
