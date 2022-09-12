using NCDC.Basic.Parsers;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Table;

namespace NCDC.ShardingParser.Command
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

        private readonly ITablesContext _tablesContext;
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

        public virtual ITablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
