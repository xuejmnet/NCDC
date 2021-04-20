using System.Collections.Generic;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.ParserBinder.Segment.Table;

namespace ShardingConnector.ParserBinder.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:52:35
* @Email: 326308290@qq.com
*/
    public interface ITableAvailable
    {
        
        ICollection<SimpleTableSegment> GetAllTables();
    
        TablesContext GetTablesContext();
    }
}