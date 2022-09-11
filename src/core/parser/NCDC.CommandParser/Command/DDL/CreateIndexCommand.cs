using System;
using NCDC.CommandParser.Segment.DDL.Index;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Command.DDL
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