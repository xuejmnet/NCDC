using System;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:37:55
* @Email: 326308290@qq.com
*/
    public sealed class AlterIndexCommand:DDLCommand
    {
        
        public IndexSegment Index { get; set; }
    
        public SimpleTableSegment Table{ get; set; }
    }
}