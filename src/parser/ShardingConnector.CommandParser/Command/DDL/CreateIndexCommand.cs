using System;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:43:57
* @Email: 326308290@qq.com
*/
    public sealed class CreateIndexCommand:DDLCommand
    {
        
        public IndexSegment Index { get; set; }
    
        public SimpleTableSegment Table{ get; set; }
    }
}