using System;
using System.Collections.Generic;
using NCDC.CommandParser.Segment.DDL.Index;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:51:08
* @Email: 326308290@qq.com
*/
    public sealed class DropIndexCommand:DDLCommand
    {
        public readonly ICollection<IndexSegment> Indexes = new LinkedList<IndexSegment>();
    
        public SimpleTableSegment Table { get; set; }
    }
}