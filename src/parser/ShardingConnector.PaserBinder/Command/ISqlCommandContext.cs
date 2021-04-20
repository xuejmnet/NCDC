using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Segment.Table;

namespace ShardingConnector.ParserBinder.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:24:51
* @Email: 326308290@qq.com
*/
    public interface ISqlCommandContext<out T> where T:ISqlCommand
    {
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        T GetSqlCommand();
        /// <summary>
        /// 获取表上下文
        /// </summary>
        /// <returns></returns>
        TablesContext GetTablesContext();
    }
}